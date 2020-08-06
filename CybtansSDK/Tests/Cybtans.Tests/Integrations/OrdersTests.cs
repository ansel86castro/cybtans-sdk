using Cybtans.AspNetCore;
using Cybtans.Refit;
using Cybtans.Services;
using Cybtans.Tests.Clients;
using Cybtans.Tests.Entities.EntityFrameworkCore;
using Cybtans.Tests.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Cybtans.Tests.Integrations
{
    public class OrdersTests: IClassFixture<IntegrationFixture>
    {
        IntegrationFixture _fixture;
        ITestOutputHelper _testOutputHelper;        
        IOrderService _service;

        public OrdersTests(IntegrationFixture fixture, ITestOutputHelper testOutputHelper)
        {
            _fixture = fixture;
            _testOutputHelper = testOutputHelper;
            _service = fixture.GetClient<IOrderService>();
        }

        private async Task<OrderDto> CreateOrderInternal()
        {
            var order = new OrderDto
            {                
                OrderStateId = 1,
                CustomerId = RepositoryFixture.CustomerId,
                Description = "Test Order",
                Items = new List<OrderItemDto>
                {
                    new OrderItemDto
                    {
                         ProductName = "Product 1",
                         Discount = 0,
                         Price = 10
                    }
                }
            };

            return await _service.Create(order);            
        }

        [Fact]
        public async Task CreateOrder()
        {
            var createdOrder = await CreateOrderInternal();
            var order = await _service.Get(createdOrder.Id);

            Assert.NotNull(order);
            Assert.NotNull(order.Customer);
            Assert.NotNull(order.Customer.CustomerProfile);
            Assert.Equal(RepositoryFixture.CustomerId, order.CustomerId);
            Assert.Equal("Test Order", order.Description);

            Assert.Single(order.Items);

            Assert.Equal("Product 1", order.Items.First().ProductName);
        }

        [Fact]
        public async Task CreateOrderWithNewCustomer()
        {
            var createdOrder = await _service.Create(new OrderDto
            {
                OrderStateId = 1,
                Customer = new CustomerDto
                {
                    Name = "Test2",
                    FirstLastName = "Test2",
                    SecondLastName = "Test2",
                    CustomerProfile = new CustomerProfileDto
                    {
                        Name = "Test Profile2"
                    }
                },
                Description = "Test Order2",
                Items = new List<OrderItemDto>
                    {
                        new OrderItemDto
                        {
                             ProductName = "Product 12",
                             Discount = 0,
                             Price = 10
                        }
                    }
            });

            var order = await _service.Get(createdOrder.Id);

            Assert.NotNull(order);
            Assert.NotNull(order.Customer);
            Assert.NotNull(order.Customer.CustomerProfile);
            
            Assert.Equal(createdOrder.CustomerId, order.CustomerId);
            Assert.Equal("Test Order2", order.Description);

            Assert.Single(order.Items);

            Assert.Equal("Product 12", order.Items.First().ProductName);
        }

        [Fact]
        public async Task AddOrderItem()
        {
            var createResult = await CreateOrderInternal();
            var order = await _service.Get(createResult.Id);
            order.Items.Add(new OrderItemDto { ProductName = "Updated Product", Price = 20 });

            var updateResult = await _service.Update(new UpdateOrderRequest
            {
                Id = order.Id,
                Value = order
            });

            Assert.Equal(2, updateResult.Items.Count);
            Assert.NotNull(updateResult.Items.FirstOrDefault(x => x.ProductName == "Updated Product"));
        }

        [Fact]
        public async Task RemoveOrderItem()
        {
            var createResult = await CreateOrderInternal();

            var order = await _service.Get(createResult.Id);

            order.Items.Remove(order.Items.First(x => x.ProductName == "Product 1"));            

            var updateResult = await _service.Update(new UpdateOrderRequest
            {
                Id = order.Id,
                Value = order
            });
            
            Assert.Null(updateResult.Items.FirstOrDefault(x => x.ProductName == "Product 1"));
        }

        [Fact]
        public async Task UpdateOrder()
        {
            var createResult = await CreateOrderInternal();

            var order = await _service.Get(createResult.Id);

            order.Description = "Updated";
            order.Customer = new CustomerDto
            {
                Name = "Update Customer",
                FirstLastName = "Update Customer",
                SecondLastName = "Update Customer",
                CustomerProfile = new CustomerProfileDto
                {
                    Name = "Update Test Customer"
                }
            };            

            var updateResult = await _service.Update(new UpdateOrderRequest
            {
                Id = order.Id,
                Value = order
            });

            Assert.NotEmpty(updateResult.Items);
            Assert.NotNull(updateResult.Items.FirstOrDefault(x => x.ProductName == "Product 1"));
        }

        [Fact]
        public async Task ShouldGetValidationResult()
        {
            var exception = await Assert.ThrowsAsync<ApiException>(async () => await _service.Create(new OrderDto
            {
                CustomerId = RepositoryFixture.CustomerId,
                Items = new List<OrderItemDto>
                {
                    new OrderItemDto
                    {
                         ProductName = "Product 1",
                         Discount = 0,
                         Price = 10
                    }
                }
            }));

            Assert.NotNull(exception);

            var result = exception.ToErrorInfo() as ValidationResult;
            Assert.NotNull(result);
            Assert.Equal("One or more validation errors occurred.", result.ErrorMessage);
            Assert.Equal(2, result.Errors.Count);
            Assert.Equal("Description can not be empty", result.Errors["Description"].First());
        }

        [Fact]
        public async Task ShouldGetInnerSQLErrorMessage()
        {
            var createOrder = await CreateOrderInternal();            

            var exception = await Assert.ThrowsAsync<ApiException>(async () => await _service.Create(new OrderDto
            {
                Id = createOrder.Id,
                Description = "string",
                OrderStateId = 1
            }));

            Assert.NotNull(exception);

            var result = exception.ToErrorInfo() as ValidationResult;
            Assert.NotNull(result);
            Assert.NotEmpty(result.ErrorMessage);
        }

    }
}
