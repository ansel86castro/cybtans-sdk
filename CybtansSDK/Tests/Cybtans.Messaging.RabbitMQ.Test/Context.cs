using Cybtans.Entities;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace Cybtans.Messaging.RabbitMQ.Test
{

    public class Invoice:Entity
    {
        public int Id { get; set; }

        public string Code { get; set; }

        public List<InvoiceItems> Items { get; set; }
    }

    public class InvoiceItems
    {
        public int Id { get; set; }

        public string Product { get; set; }

        public float Price { get; set; }

    }

    public class Customer
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }

    public class TestContext : DbContext
    {
        public TestContext(DbContextOptions<TestContext> options) : base(options)
        {
        }

        public DbSet<Invoice> Invoices { get; set; }

        public DbSet<InvoiceItems> InvoicesItems { get; set; }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<EntityEventLog> Events { get; set; }

        public static TestContext Create(DbConnection connection = null)
        {
            var builder = new DbContextOptionsBuilder<TestContext>();
            builder.UseSqlite(connection ?? CreateInMemoryDatabase());
            return new TestContext(builder.Options);
        }       

        public static DbConnection CreateInMemoryDatabase()
        {
            var connection = new SqliteConnection("Data Source=Test;Mode=Memory;Cache=Shared");
            connection.Open();

            return connection;
        }
    }
}
