using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly ILogger<InventoryController> _logger;

        public InventoryController(ILogger<InventoryController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public int Post(Inventory inventory)
        {
            throw new NotImplementedException();

            _logger.LogInformation($"Updated inventory for: {inventory.ProductName}");
            return 2;
        }
        
        [HttpDelete("{id}")]
        public void Delete(int id) =>
            _logger.LogInformation($"Deleted inventory");
    }
}
