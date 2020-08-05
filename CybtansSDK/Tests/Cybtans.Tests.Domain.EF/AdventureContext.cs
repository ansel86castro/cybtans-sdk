using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

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

        public DbSet<CustomerProfile> CustomerProfiles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite(CreateInMemoryDatabase());
                optionsBuilder.EnableSensitiveDataLogging(true);
            }
            base.OnConfiguring(optionsBuilder);
        }

        public static DbConnection CreateInMemoryDatabase(string database = "AdventureWorks")
        {
            var connection = new SqliteConnection($"Data Source={database};Mode=Memory;Cache=Shared");
            connection.Open();

            return connection;
        }
    }
}
