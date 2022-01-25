using Microsoft.EntityFrameworkCore;
using Shared;

namespace Notifier
{
    public class NotifierServiceDbContext : DbContext
    {
        public DbSet<Order> Notifiers { get; set; }
        public DbSet<Account> Accounts { get; set; }

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
