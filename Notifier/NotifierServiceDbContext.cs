using Microsoft.EntityFrameworkCore;

namespace Notifier
{
    public class NotifierServiceDbContext : DbContext
    {
        public DbSet<Notifier> Notifiers { get; set; }

        public NotifierServiceDbContext()
        {
            Database.EnsureCreated();
        }

        public NotifierServiceDbContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
