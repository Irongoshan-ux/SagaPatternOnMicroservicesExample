using Microsoft.AspNetCore.Mvc;
using Shared;

namespace Saga.Orchestrator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrchestrationController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<OrchestrationController> _logger;

        public OrchestrationController(IHttpClientFactory httpClientFactory, ILogger<OrchestrationController> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Post(OrderDto orderDto)
        {
            var orderAfterWithdrawMoney = GetOrderAfterWithdrawMoney(orderDto);

            if (orderAfterWithdrawMoney.Account.Money < decimal.Zero)
                return BadRequest(new OrderResponse
                {
                    OrderId = orderAfterWithdrawMoney.Id
                });

            var orderResult = await CreateOrderAsync(orderAfterWithdrawMoney);
            var (orderId, orderClient) = orderResult;

            if (string.IsNullOrEmpty(orderId)) return BadRequest(new OrderResponse());

            string inventoryId = await UpdateInventoryAsync(orderAfterWithdrawMoney, orderClient, orderId);
            if (string.IsNullOrEmpty(inventoryId)) return BadRequest(new OrderResponse());

            string notificationId = await SendNotificationAsync(orderAfterWithdrawMoney);

            _logger.LogInformation($"Order: {orderId}; Inventory: {inventoryId}; Notification: {notificationId}");

            return Ok(new OrderResponse { OrderId = orderId });
        }

        private static Order GetOrderAfterWithdrawMoney(OrderDto orderDto) =>
            new Order
            {
                Id = Guid.NewGuid().ToString(),
                ProductName = orderDto.ProductName,
                Price = orderDto.Price,
                Account = new Account
                {
                    Id = Guid.NewGuid().ToString(),
                    Money = orderDto.Account.Money - orderDto.Price,
                    Name = orderDto.Account.Name
                }
            };

        private async Task<string> SendNotificationAsync(Order order)
        {
            var notificationClient = _httpClientFactory.CreateClient("Notifier");
            var notificationResponse = await notificationClient.PostAsJsonAsync("/api/notifier", order);
            var notificationId = await notificationResponse.Content.ReadAsStringAsync();
            return notificationId;
        }
        
        private async Task<(string, HttpClient)> CreateOrderAsync(Order orderAfterWithdrawMoney)
        {
            var orderClient = _httpClientFactory.CreateClient("Order");
            var orderResponse = await orderClient.PostAsJsonAsync("/api/order", orderAfterWithdrawMoney);

            if (orderResponse.StatusCode != System.Net.HttpStatusCode.OK)
                return (string.Empty, orderClient);

            var orderId = await orderResponse.Content.ReadAsStringAsync();

            return (orderId, orderClient);
        }

        private async Task<string> UpdateInventoryAsync(Order order, HttpClient orderClient, string orderId)
        {
            string? inventoryId;

            try
            {
                var inventoryClient = _httpClientFactory.CreateClient("Inventory");
                var inventoryResponse = await inventoryClient.PostAsJsonAsync("/api/inventory", order);

                if (inventoryResponse.StatusCode != System.Net.HttpStatusCode.OK)
                    throw new Exception(inventoryResponse.ReasonPhrase);

                inventoryId = await inventoryResponse.Content.ReadAsStringAsync();
            }
            catch // Compensating transaction
            {
                await orderClient.DeleteAsync($"/api/order/{orderId}");
                
                _logger.LogInformation("There is some problem with inventory Service");
                
                return string.Empty;
            }

            return inventoryId;
        }
    }
}