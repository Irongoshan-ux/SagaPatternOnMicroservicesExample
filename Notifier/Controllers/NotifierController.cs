using Microsoft.AspNetCore.Mvc;

namespace Notifier.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotifierController : ControllerBase
    {
        private readonly ILogger<NotifierController> _logger;

        public NotifierController(ILogger<NotifierController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public int Post(Notifier notifier)
        {
            _logger.LogInformation($"Updated inventory for: {notifier.ProductName}");
            return 3;
        }

        [HttpDelete("{id}")]
        public void Delete(int id) =>
            _logger.LogInformation($"Deleted inventory");
    }
}