using Microsoft.EntityFrameworkCore;

namespace Inventory
{
    public class InventoryServiceDbContext : DbContext
    {
        public DbSet<Inventory> Inventories { get; set; }

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
