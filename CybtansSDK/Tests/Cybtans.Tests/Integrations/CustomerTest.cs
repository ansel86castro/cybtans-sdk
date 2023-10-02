using Cybtans.Clients;
using Cybtans.Testing;
using Cybtans.Tests.Clients;
using Cybtans.Tests.Entities.EntityFrameworkCore;
using Cybtans.Tests.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using Cybtans.Common;
using Cybtans.Tests.Services;

namespace Cybtans.Tests.Integrations
{
    public class CustomerTest : IClassFixture<IntegrationFixture>
    {
        IntegrationFixture _fixture;
        ITestOutputHelper _testOutputHelper;
        ICustomerEventService _customerEventService;
        ICustomerService _service;
        public CustomerTest(IntegrationFixture fixture, ITestOutputHelper testOutputHelper)
        {
            _fixture = fixture;
            _testOutputHelper = testOutputHelper;
            _customerEventService = fixture.GetClient<ICustomerEventService>();
            _service = fixture.GetClient<ICustomerService>();
        }

        [Fact]
        public async Task CreateCustomer()
        {           
            var customer = await CreateCustomerInternal("Integration");
            Assert.NotEqual(new DateTime(), customer.CreateDate);            

            var result =await _service.GetAll(new GetAllRequest
            {
                 Filter = "name eq 'Integration'"
            });

            Assert.True(result.TotalCount > 0);
            Assert.NotEmpty(result.Items);

            var customerEvent = await _customerEventService.Get(customer.Id);
            Assert.NotNull(customerEvent);
            Assert.NotEmpty(customerEvent.FullName);
            Assert.Equal("Integration Integration", customerEvent.FullName.Trim());
        }

        [Fact]
        public async Task UpdateCustomer()
        {
            var customer = await CreateCustomerInternal("Integration");
            customer.FirstLastName = "Foo";
            customer.SecondLastName = "Baar";
            
            var result = await _service.Update(new UpdateCustomerRequest
            {
                Id = customer.Id,
                Value = customer
            });

            Assert.NotNull(result);
            Assert.Equal("Baar", result.SecondLastName);
            Assert.NotNull(result.UpdateDate);

            var customerEvent = await _customerEventService.Get(customer.Id);
            Assert.NotNull(customerEvent);
             Assert.Equal("Integration Foo Baar", customerEvent.FullName.Trim());
        }

        [Fact]
        public async Task DeleteCustomer()
        {            
            var customer = await CreateCustomerInternal("Integration");
            var result = await _service.GetAll(new GetAllRequest
            {
                Filter = $"id eq '{customer.Id}'"
            });
            Assert.Equal(1, result.TotalCount);

           
            await _service.Delete(customer.Id);

            result = await _service.GetAll(new GetAllRequest
            {
                Filter = $"id eq '{customer.Id}'"
            });
            Assert.Equal(0, result.TotalCount);
            Assert.Empty(result.Items);

            var ex = await Assert.ThrowsAsync<ApiException>(() => _customerEventService.Get(customer.Id));
            Assert.Equal(HttpStatusCode.NotFound, ex.StatusCode);
        }

        private async Task<CustomerDto> CreateCustomerInternal(string name)
        {
            var customer = new CustomerDto
            {
                Name = name,
                FirstLastName = name,                
                CustomerProfile = new CustomerProfileDto
                {
                    Name = $"{name} Profile"
                }
            };
            
            var result = await _service.Create(customer);

            Assert.NotNull(result);
            Assert.True(result.Id != Guid.Empty);
            Assert.NotNull(result.CustomerProfile);
            Assert.Equal(name, customer.Name);
            Assert.Equal(name, customer.FirstLastName);
            Assert.Null(customer.SecondLastName);
            Assert.Equal($"{name} Profile", customer.CustomerProfile.Name);

            return result;

        }

        [Fact]
        public async Task GetCustomer()
        {
            var customer = await _service.Get(RepositoryFixture.CustomerId);

            Assert.NotNull(customer);
            Assert.NotNull(customer.CustomerProfile);
            Assert.Equal(RepositoryFixture.CustomerId, customer.Id);
        }

    }
}
