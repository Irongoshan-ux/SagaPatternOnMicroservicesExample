using Microsoft.AspNetCore.Mvc;

namespace Order.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly ILogger<OrderController> _logger;

        public OrderController(ILogger<OrderController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public int Post(Order order)
        {
            _logger.LogInformation($"Created new order: {order.ProductName}");

            return 1;
        }

        [HttpDelete("{id}")]
        public void Delete(int id) =>
            _logger.LogInformation($"Deleted order: {id}");
    }
}