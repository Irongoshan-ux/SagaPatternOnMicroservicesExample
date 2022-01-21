using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Saga.Orchestrator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<OrderController> _logger;

        public OrderController(IHttpClientFactory httpClientFactory, ILogger<OrderController> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        [HttpPost]
        public async Task<OrderResponse> Post(Order order)
        {
            // Create order
            var orderClient = _httpClientFactory.CreateClient("Order");
            var orderResponse = await orderClient.PostAsJsonAsync("/api/order", order);
            var orderId = await orderResponse.Content.ReadAsStringAsync();

            // Update Inventory
            string inventoryId = await UpdateInventoryAsync(order, orderClient, orderId);

            if (string.IsNullOrEmpty(inventoryId)) return new OrderResponse();

            // Send notification
            string notificationId = await SendNotificationAsync(order);

            _logger.LogInformation($"Order: {orderId}; Inventory: {inventoryId}; Notification: {notificationId}");

            return new OrderResponse { OrderId = orderId };
        }

        private async Task<string> SendNotificationAsync(Order order)
        {
            var notificationClient = _httpClientFactory.CreateClient("Notifier");
            var notificationResponse = await notificationClient.PostAsJsonAsync("/api/notifier", order);
            var notificationId = await notificationResponse.Content.ReadAsStringAsync();
            return notificationId;
        }

        private async Task<string> UpdateInventoryAsync(Order order, HttpClient orderClient, string orderId)
        {
            var inventoryId = string.Empty;
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
