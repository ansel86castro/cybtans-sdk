
using System;
using AutoMapper;
using Cybtans.Entities;
using Cybtans.Services;
using Microsoft.Extensions.Logging;
using Cybtans.Tests.Domain;
using Cybtans.Tests.Models;

namespace Cybtans.Tests.Services
{
    [RegisterDependency(typeof(ICustomerEventService))]
    public class CustomerEventService : CrudService<CustomerEvent, Guid, CustomerEventDto, GetCustomerEventRequest, GetAllRequest, GetAllCustomerEventResponse, UpdateCustomerEventRequest, CreateCustomerEventRequest, DeleteCustomerEventRequest>, ICustomerEventService
    {
        public CustomerEventService(IRepository<CustomerEvent, Guid> repository, IUnitOfWork uow, IMapper mapper, ILogger<CustomerEventService> logger)
            : base(repository, uow, mapper, logger) { }                
    }
}