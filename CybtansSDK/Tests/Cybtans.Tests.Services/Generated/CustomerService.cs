
using System;
using AutoMapper;
using Cybtans.Entities;
using Cybtans.Services;
using Microsoft.Extensions.Logging;
using Cybtans.Tests.Domain;
using Cybtans.Tests.Models;

namespace Cybtans.Tests.Services
{
    [RegisterDependency(typeof(ICustomerService))]
    public class CustomerService : CrudService<Customer, Guid, CustomerDto, GetCustomerRequest, GetAllRequest, GetAllCustomerResponse, UpdateCustomerRequest, CreateCustomerRequest, DeleteCustomerRequest>, ICustomerService
    {
        public CustomerService(IRepository<Customer, Guid> repository, IUnitOfWork uow, IMapper mapper, ILogger<CustomerService> logger)
            : base(repository, uow, mapper, logger) { }                
    }
}