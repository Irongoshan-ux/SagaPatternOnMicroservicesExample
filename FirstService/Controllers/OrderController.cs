using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Order.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly ILogger<OrderController> _logger;
        private readonly OrderServiceDbContext _context;

        public OrderController(ILogger<OrderController> logger, OrderServiceDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpPost]
        public string Post(Order order)
        {
            if(!Guid.TryParse(order.Id, out Guid orderId))
                order.Id = Guid.NewGuid().ToString();

            EntityEntry<Order> addedOrder;
            
            try
            {
                addedOrder = _context.Orders.Add(order);

                _logger.LogInformation($"Created new order: {order.ProductName}");
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"There is exception with order: {order.ProductName}");
                throw new Exception(ex.Message);
            }

            _context.SaveChanges();

            return addedOrder.Entity.Id;
        }

        [HttpDelete("{id}")]
        public void Delete(string id)
        {
            _logger.LogInformation($"Deleted order: {id}");
            
            var order = _context.Orders.FirstOrDefault(x => x.Id == id);

            if (order is not null)
            {
                _context.Orders.Remove(order);
                _context.SaveChanges();
            }
        }
    }
}