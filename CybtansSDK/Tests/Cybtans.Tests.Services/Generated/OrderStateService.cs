
using System;
using AutoMapper;
using Cybtans.Entities;
using Cybtans.Services;
using Microsoft.Extensions.Logging;
using Cybtans.Tests.Domain;
using Cybtans.Tests.Models;

namespace Cybtans.Tests.Services
{
    [RegisterDependency(typeof(IOrderStateService))]
    public class OrderStateService : CrudService<OrderState, int, OrderStateDto, GetOrderStateRequest, GetAllRequest, GetAllOrderStateResponse, UpdateOrderStateRequest, CreateOrderStateRequest, DeleteOrderStateRequest>, IOrderStateService
    {
        public OrderStateService(IRepository<OrderState, int> repository, IUnitOfWork uow, IMapper mapper, ILogger<OrderStateService> logger)
            : base(repository, uow, mapper, logger) { }                
    }
}