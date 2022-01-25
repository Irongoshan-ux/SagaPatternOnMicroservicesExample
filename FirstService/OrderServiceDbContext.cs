using Microsoft.EntityFrameworkCore;
using Shared;

namespace Order
{
    public class OrderServiceDbContext : DbContext
    {
        public DbSet<Shared.Order> Orders { get; set; }
        public DbSet<Account> Accounts { get; set; }

        public OrderServiceDbContext()
        {
            Database.EnsureCreated();
        }

        public OrderServiceDbContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
