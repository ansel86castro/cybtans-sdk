
using System;
using AutoMapper;
using Cybtans.Entities;
using Cybtans.Services;
using Microsoft.Extensions.Logging;
using Cybtans.Test.Domain;
using Cybtans.Tests.Models;
using System.Threading.Tasks;
using System.Net;

namespace Cybtans.Tests.Services
{
    [RegisterDependency(typeof(IOrderService))]
    public class OrderService : CrudService<Order, Guid, OrderDto, GetOrderRequest, GetAllRequest, GetAllOrderResponse, UpdateOrderRequest, DeleteOrderRequest>, IOrderService
    {
        public OrderService(IRepository<Order, Guid> repository, IUnitOfWork uow, IMapper mapper, ILogger<OrderService> logger)
            : base(repository, uow, mapper, logger) { }

        public Task Baar()
        {
            throw new CybtansException(HttpStatusCode.NotAcceptable, "Method Baar no allowed");
        }

        public Task Foo()
        {
            throw new NotImplementedException();
        }
    }
}