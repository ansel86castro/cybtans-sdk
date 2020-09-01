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

            if (!context.OrderStates.Any())
            {
                context.OrderStates.AddRange(
                    new OrderState { Id = 1, Name = "Draft" },
                    new OrderState { Id = 2, Name = "Submitted" },
                    new OrderState { Id = 3, Name = "Processed" },
                    new OrderState { Id = 4, Name = "Shipped" },
                    new OrderState { Id = 5, Name = "Delivered" });

                context.Customers.Add(new Customer
                {                    
                    Name = "Test",
                    FirstLastName = "Test",
                    SecondLastName = "Test",
                    CustomerProfile = new CustomerProfile
                    {                 
                        Name = "Test Profile"
                    }
                });

                await context.SaveChangesAsync();

                context.Orders.Add(new Order
                {
                    OrderStateId = 1,
                    CustomerId = context.Customers.FirstOrDefault(x=>x.Name == "Test").Id,
                    Description = "Order 1",
                    OrderType = OrderTypeEnum.Normal,
                    CreateDate = DateTime.Now,
                    Items = new List<OrderItem>
                    {
                        new OrderItem
                        {
                                ProductName = "Product 1",
                                Discount = 0,
                                Price = 10
                        }
                    }
                });

                await context.SaveChangesAsync();
            }
        }
    }
}
