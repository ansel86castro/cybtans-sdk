using Cybtans.Entities;
using Cybtans.Messaging;
using Ordering.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Services
{
    public class OrdersServiceImpl : OrdersService
    {
        static List<Order> orders = new List<Order>();

        IRepository<Order, Guid> _ordersReposiory;
        private IMessageQueue _queue;

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

        public OrdersServiceImpl(IRepository<Order, Guid> ordersRepository, IMessageQueue queue)
        {
            _ordersReposiory = ordersRepository;
            _queue = queue;
        }

        public override async Task<Order> CreateOrder(Order request)
        {
            _ordersReposiory.Add(request);
            await _ordersReposiory.UnitOfWork.SaveChangesAsync();

            return request;
        }

        public override async Task<Order> GetOrder(GetOrderRequest request)
        {
            return await _ordersReposiory.Get(request.Id);
        }

        public override Task<GetOrdersResponse> GetOrdersByUser(GetOrderByUserRequest request)
        {
            return Task.FromResult(new GetOrdersResponse
            {
                Orders = _ordersReposiory.Where(x => x.UserId == request.UserId).ToList()
            });
        }
    }
}
