using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Shared;

namespace Notifier.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotifierController : ControllerBase
    {
        private readonly ILogger<NotifierController> _logger;
        private readonly NotifierServiceDbContext _context;

        public NotifierController(ILogger<NotifierController> logger, NotifierServiceDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpPost]
        public string Post(Order orderForNotifier)
        {
            EntityEntry<Order> addedOrder;

            try
            {
                addedOrder = _context.Notifiers.Add(orderForNotifier);

                _logger.LogInformation($"Updated notification for: {orderForNotifier.ProductName}");
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"There is exception with order: {orderForNotifier.ProductName}");
                throw new Exception(ex.Message);
            }

            _context.SaveChanges();

            return addedOrder.Entity.Id;
        }

        [HttpDelete("{id}")]
        public void Delete(string id)
        {
            _logger.LogInformation($"Deleted notification");

            var notifier = _context.Notifiers.FirstOrDefault(x => x.Id == id);

            if (notifier is not null)
            {
                _context.Notifiers.Remove(notifier);
                _context.SaveChanges();
            }
        }
    }
}