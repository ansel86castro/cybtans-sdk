using Cybtans.Entities;
using Cybtans.Entities.EntityFrameworkCore;
using Cybtans.Test.Domain;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace Cybtans.Tests.Domain.EF
{
    public partial class AdventureContext : DbContext, IEntityEventLogContext
    {
        public AdventureContext()
        {
        }

        public AdventureContext(DbContextOptions<AdventureContext> options)
            : base(options)
        {
        }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderItem> OrderItems { get; set; }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<CustomerEvent> CustomerEvents { get; set; }

        public DbSet<OrderState> OrderStates { get; set; }

        public DbSet<CustomerProfile> CustomerProfiles { get; set; }

        public DbSet<EntityEventLog> EntityEventLogs { get; set; }

        public DbSet<SoftDeleteOrder> SoftDeleteOrders { get; set; }

        public DbSet<SoftDeleteOrderItem> SoftDeleteOrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.AddSoftDeleteQueryFilters();
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
            //var connection = new SqliteConnection($"Data Source={database};Mode=Memory;Cache=Shared");
            var connection = new SqliteConnection($"Data Source =:memory:");
            connection.Open();

            return connection;
        }
    }
}
