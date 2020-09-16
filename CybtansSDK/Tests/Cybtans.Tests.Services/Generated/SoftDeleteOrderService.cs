
using System;
using AutoMapper;
using Cybtans.Entities;
using Cybtans.Services;
using Microsoft.Extensions.Logging;
using Cybtans.Test.Domain;
using Cybtans.Tests.Models;

namespace Cybtans.Tests.Services
{
    [RegisterDependency(typeof(ISoftDeleteOrderService))]
    public class SoftDeleteOrderService : CrudService<SoftDeleteOrder, Guid, SoftDeleteOrderDto, GetSoftDeleteOrderRequest, GetAllRequest, GetAllSoftDeleteOrderResponse, UpdateSoftDeleteOrderRequest, CreateSoftDeleteOrderRequest, DeleteSoftDeleteOrderRequest>, ISoftDeleteOrderService
    {
        public SoftDeleteOrderService(IRepository<SoftDeleteOrder, Guid> repository, IUnitOfWork uow, IMapper mapper, ILogger<SoftDeleteOrderService> logger)
            : base(repository, uow, mapper, logger) { }                
    }
}