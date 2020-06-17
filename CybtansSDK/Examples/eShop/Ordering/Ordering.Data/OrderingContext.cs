using Cybtans.Entities;
using Cybtans.Entities.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Ordering.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ordering.Data
{
    public class OrderingContext : DbContext, IEntityEventLogContext
    {
        public OrderingContext() { }

        public OrderingContext(DbContextOptions<OrderingContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=Ordering;Trusted_Connection=True;MultipleActiveResultSets=true");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>(buider =>
            {
                buider.Property(x => x.Id).ValueGeneratedOnAdd();

                buider.HasMany(x => x.Items)
                .WithOne()
                .HasForeignKey(x => x.OrderId);

            });


            modelBuilder.Entity<OrderItem>(buider =>
            {
                buider.Property(x => x.Id).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<Order>().HasData(
            new Order
            {
                Id = Guid.NewGuid(),
                Name = "Order 1",
                CreateDate = DateTime.Now,                
                Total = 12.30f,
                Tax = 1.80f,
                SubTotal = 10.50f,
                UserId = 1

            },
            new Order
            {
                Id = Guid.NewGuid(),
                Name = "Order 2",
                CreateDate = DateTime.Now,
                Total = 12.30f,
                Tax = 1.80f,
                SubTotal = 10.50f,
                UserId = 2

            });
        }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderItem> OrderItems { get; set; }

        public DbSet<EntityEventLog> EntityEventLogs { get; set; }

    }
}
