using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Shared;

namespace Inventory.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly ILogger<InventoryController> _logger;
        private readonly InventoryServiceDbContext _context;

        public InventoryController(ILogger<InventoryController> logger, InventoryServiceDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpPost]
        public string Post(Order orderForInventory)
        {
            EntityEntry<Order> addedOrder;

            try
            {
                addedOrder = _context.Orders.Add(orderForInventory);
                
                _logger.LogInformation($"Created new order: {orderForInventory.ProductName}");
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"There is exception with order: {orderForInventory.ProductName}");
                throw new Exception(ex.Message);
            }

            _context.SaveChanges();

            return addedOrder.Entity.Id;
        }
        
        [HttpDelete("{id}")]
        public void Delete(string id)
        {
            _logger.LogInformation($"Deleted inventory");

            var inventory = _context.Orders.FirstOrDefault(x => x.Id == id);

            if (inventory is not null)
                _context.Orders.Remove(inventory);
        }
    }
}
