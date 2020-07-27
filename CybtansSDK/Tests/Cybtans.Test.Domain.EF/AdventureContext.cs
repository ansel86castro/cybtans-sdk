using Microsoft.EntityFrameworkCore;

namespace Cybtans.Test.Domain.EF
{
    public partial class AdventureContext : DbContext
    {
        public AdventureContext()
        {
        }

        public AdventureContext(DbContextOptions<AdventureContext> options)
            : base(options)
        {
        }         
        
        public DbSet<Order> Ordes { get; set; }

        public DbSet<OrderItem> OrderItems { get; set; }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<OrderState> OrderStates { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
        }        
    }
}
