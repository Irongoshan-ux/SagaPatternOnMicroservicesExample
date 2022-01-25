using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;

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
        public string Post(Inventory inventory)
        {
            EntityEntry<Inventory> addedOrder;

            try
            {
                addedOrder = _context.Inventories.Add(inventory);
                
                _logger.LogInformation($"Created new order: {inventory.ProductName}");
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"There is exception with order: {inventory.ProductName}");
                throw new Exception(ex.Message);
            }

            _context.SaveChanges();

            return addedOrder.Entity.Id;
        }
        
        [HttpDelete("{id}")]
        public void Delete(string id)
        {
            _logger.LogInformation($"Deleted inventory");

            var inventory = _context.Inventories.FirstOrDefault(x => x.Id == id);

            if (inventory is not null)
                _context.Inventories.Remove(inventory);
        }
    }
}
