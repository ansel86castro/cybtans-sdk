using Cybtans.Testing;
using Cybtans.Tests.Clients;
using Cybtans.Tests.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Cybtans.Tests.Integrations
{
    public class CustomerTest : IClassFixture<IntegrationFixture>
    {
        IntegrationFixture _fixture;
        ITestOutputHelper _testOutputHelper;
        HttpClient _httpClient;

        public CustomerTest(IntegrationFixture fixture, ITestOutputHelper testOutputHelper)
        {
            _fixture = fixture;
            _testOutputHelper = testOutputHelper;
            _httpClient = fixture.CreateClient();
        }

        [Fact]
        public async Task CreateCustomer()
        {           
            var customer = await CreateCustomerInternal("Integration");
            Assert.NotEqual(new DateTime(), customer.CreateDate);

            var client = _fixture.GetClient<ICustomerService>(_httpClient);

            var result =await client.GetAll(new GetAllRequest
            {
                 Filter = "name eq 'Integration'"
            });

            Assert.True(result.TotalCount > 0);
            Assert.NotEmpty(result.Items);
        }

        [Fact]
        public async Task UpdateCustomer()
        {
            var customer = await CreateCustomerInternal("Integration");
            customer.SecondLastName = "LastName";

            var client = _fixture.GetClient<ICustomerService>(_httpClient);
            var result = await client.Update(new UpdateCustomerRequest
            {
                Id = customer.Id,
                Value = customer
            });

            Assert.NotNull(result);
            Assert.Equal("LastName", result.SecondLastName);
            Assert.NotNull(result.UpdateDate);
        }

        [Fact]
        public async Task DeleteCustomer()
        {
            var client = _fixture.GetClient<ICustomerService>(_httpClient);
            var customer = await CreateCustomerInternal("Integration");
            var result = await client.GetAll(new GetAllRequest
            {
                Filter = $"id eq '{customer.Id}'"
            });
            Assert.Equal(1, result.TotalCount);

           
            await client.Delete(customer.Id);

            result = await client.GetAll(new GetAllRequest
            {
                Filter = $"id eq '{customer.Id}'"
            });
            Assert.Equal(0, result.TotalCount);
            Assert.Empty(result.Items);
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

            var client = _fixture.GetClient<ICustomerService>(_httpClient);
            var result = await client.Create(customer);

            Assert.NotNull(result);
            Assert.True(result.Id != Guid.Empty);
            Assert.NotNull(result.CustomerProfile);
            Assert.Equal(name, customer.Name);
            Assert.Equal(name, customer.FirstLastName);
            Assert.Null(customer.SecondLastName);
            Assert.Equal($"{name} Profile", customer.CustomerProfile.Name);

            return result;

        }

    }
}
