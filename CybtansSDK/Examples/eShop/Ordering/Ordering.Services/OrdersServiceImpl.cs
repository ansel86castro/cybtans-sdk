using Ordering.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Services
{
    public class OrdersServiceImpl : OrdersService
    {
        static List<Order> orders = new List<Order>();

        static OrdersServiceImpl()
        {
            orders.Add(new Order
            {
                Id = Guid.NewGuid(),
                Name = "Order 1",
                CreateDate = DateTime.Now,
                Items = new List<OrderItem>
                {
                    new OrderItem
                    {
                            Id = 1,
                            Name = "Product 1",
                            Discount = 0,
                            Price = 10.50f
                    }
                },
                Total = 12.30f,
                Tax = 1.80f,
                SubTotal = 10.50f,
                UserId = 1

            });

            orders.Add(new Order
            {
                Id = Guid.NewGuid(),
                Name = "Order 2",
                CreateDate = DateTime.Now,
                Items = new List<OrderItem>
                {
                    new OrderItem
                    {
                            Id = 1,
                            Name = "Product 1",
                            Discount = 0,
                            Price = 10.50f
                    }
                },
                Total = 12.30f,
                Tax = 1.80f,
                SubTotal = 10.50f,
                UserId = 2

            });
        }


        public override Task<Order> CreateOrder(Order request)
        {
            request.Id = Guid.NewGuid();
            orders.Add(request);
            return Task.FromResult(request);
        }

        public override Task<Order> GetOrder(GetOrderRequest request)
        {
            return Task.FromResult(orders.FirstOrDefault(x => x.Id == request.Id));
        }

        public override Task<GetOrdersResponse> GetOrdersByUser(GetOrderByUserRequest request)
        {
            return Task.FromResult(new GetOrdersResponse
            {
                Orders = orders.Where(x => x.UserId == request.UserId).ToList()
            });
        }
    }
}
