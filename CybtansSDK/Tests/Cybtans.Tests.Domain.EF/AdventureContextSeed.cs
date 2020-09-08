using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cybtans.AspNetCore.Migrations;
using Cybtans.Test.Domain;

namespace Cybtans.Tests.Domain.EF
{
    public class AdventureContextSeed : IDbContextSeed<AdventureContext>
    {
        public async Task Seed(AdventureContext context)
        {
            context.Database.EnsureCreated();

            Random rand = new Random();
            if (!context.OrderStates.Any())
            {
                context.OrderStates.AddRange(
                    new OrderState { Id = 1, Name = "Draft" },
                    new OrderState { Id = 2, Name = "Submitted" },
                    new OrderState { Id = 3, Name = "Processed" },
                    new OrderState { Id = 4, Name = "Shipped" },
                    new OrderState { Id = 5, Name = "Delivered" });

                context.Customers.AddRange(new Customer
                {                    
                    Name = "John",
                    FirstLastName = "Doe",                    
                    CustomerProfile = new CustomerProfile
                    {                 
                        Name = "John Doe Profile"
                    }
                },
                new Customer
                {
                    Name = "Jane",
                    FirstLastName = "Doe",                    
                    CustomerProfile = new CustomerProfile
                    {
                        Name = "Jane Doe Profile"
                    }
                });

                await context.SaveChangesAsync();

                context.Orders.AddRange(
                Enumerable.Range(1, 10)
                .Select(i=>
                new Order
                {
                    OrderStateId = (i % 5) + 1,
                    CustomerId = context.Customers.FirstOrDefault(x=>x.Name == (i % 2 == 0? "John": "Jane")).Id,
                    Description = $"Order Number {i}",
                    OrderType = OrderTypeEnum.Normal,
                    CreateDate = DateTime.Now,
                    Items = new List<OrderItem>
                    {
                        new OrderItem
                        {
                                ProductName = $"Order {i} Product 1",
                                Discount = 0,
                                Price = rand.Next(100)
                        }
                    }
                }));

                await context.SaveChangesAsync();
            }
        }
    }
}
