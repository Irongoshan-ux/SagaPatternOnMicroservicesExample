using Microsoft.EntityFrameworkCore;

namespace Order
{
    public class OrderServiceDbContext : DbContext
    {
        public DbSet<Order> Orders { get; set; }

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
