using Microsoft.EntityFrameworkCore;
using Shared;

namespace Inventory
{
    public class InventoryServiceDbContext : DbContext
    {
        public DbSet<Order> Orders { get; set; }
        public DbSet<Account> Accounts { get; set; }

        public InventoryServiceDbContext()
        {
            Database.EnsureCreated();
        }

        public InventoryServiceDbContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
