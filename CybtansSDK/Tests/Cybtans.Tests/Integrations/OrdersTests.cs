using Cybtans.AspNetCore;
using Cybtans.Entities;
using Cybtans.Refit;
using Cybtans.Services;
using Cybtans.Services.Utils;
using Cybtans.Test.Domain.EF;
using Cybtans.Tests.Clients;
using Cybtans.Tests.Entities.EntityFrameworkCore;
using Cybtans.Tests.Models;
using Cybtans.Tests.Services;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.DataCollection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using AutoMapper;
using Cybtans.Test.Domain;
using System.IO;
using Newtonsoft.Json;

namespace Cybtans.Tests.Integrations
{
    public class OrdersTests: IClassFixture<IntegrationFixture>
    {
        IntegrationFixture _fixture;
        ITestOutputHelper _testOutputHelper;        
        Clients.IOrderService _service;

        public OrdersTests(IntegrationFixture fixture, ITestOutputHelper testOutputHelper)
        {
            _fixture = fixture;
            _testOutputHelper = testOutputHelper;
            _service = fixture.GetClient<Clients.IOrderService>();
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
            OrderDto createdOrder = null;
            await Assert.RaisesAnyAsync<EntityEventArg>(
                x => _fixture.OrderEvents.OnCreated += x, 
                x => _fixture.OrderEvents.OnCreated -= x, 
                async ()=>
                {
                    createdOrder = await CreateOrderInternal();
                });

            Assert.NotNull(createdOrder);
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
        public async Task ShouldGetAll()
        {
            var result = await _service.GetAll();

            using var scope = _fixture.Services.CreateScope();
            var mapper = scope.ServiceProvider.GetService<IMapper>();
            var context = scope.ServiceProvider.GetService<AdventureContext>();
            var order = await context.Orders.ToListAsync();
            var orderDto = mapper.Map<Order, OrderDto>(order.First());
            var orderDtos = await mapper.ProjectTo<OrderDto>(context.Orders).ToListAsync();

            Assert.NotNull(result);
            Assert.NotEmpty(result.Items);
            Assert.True(result.TotalCount > 0);
            Assert.Contains(result.Items, x => x.OrderType == Cybtans.Tests.Models.OrderTypeEnum.Normal);            
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

            OrderDto updateResult = null;
            await Assert.RaisesAnyAsync<EntityEventArg>(
              x => _fixture.OrderEvents.OnUpdated += x,
              x => _fixture.OrderEvents.OnUpdated -= x,
              async () =>
              {
                  updateResult = await _service.Update(new UpdateOrderRequest
                  {
                      Id = order.Id,
                      Value = order
                  });

              });

            Assert.NotNull(updateResult);

            Assert.Equal(2, updateResult.Items.Count);
            Assert.NotNull(updateResult.Items.FirstOrDefault(x => x.ProductName == "Updated Product"));
        }

        [Fact]
        public async Task RemoveOrderItem()
        {
            var createResult = await CreateOrderInternal();

            var order = await _service.Get(createResult.Id);

            order.Items.Remove(order.Items.First(x => x.ProductName == "Product 1"));

            OrderDto updateResult = await _service.Update(new UpdateOrderRequest
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

            OrderDto updateResult = null;
            await Assert.RaisesAnyAsync<EntityEventArg>(
              x => _fixture.OrderEvents.OnUpdated += x,
              x => _fixture.OrderEvents.OnUpdated -= x,
              async () =>
              {
                  updateResult = await _service.Update(new UpdateOrderRequest
                  {
                      Id = order.Id,
                      Value = order
                  });

              });
            Assert.NotNull(updateResult);

            Assert.NotEmpty(updateResult.Items);
            Assert.NotNull(updateResult.Items.FirstOrDefault(x => x.ProductName == "Product 1"));
        }

        [Fact]
        public async Task DeleteOrder()
        {
            var order = await CreateOrderInternal();          
            await Assert.RaisesAnyAsync<EntityEventArg>(
              x => _fixture.OrderEvents.OnDeleted += x,
              x => _fixture.OrderEvents.OnDeleted -= x,
              async () =>
              {
                  await _service.Delete(order.Id);
              });

            var exception = await Assert.ThrowsAsync<ApiException>(() => _service.Delete(order.Id));
            Assert.Equal(System.Net.HttpStatusCode.NotFound, exception.StatusCode);
        }

        [Fact]
        public async Task Get_Should_ThrowNotFound()
        {
            var exception = await Assert.ThrowsAsync<ApiException>(() => _service.Get(Guid.NewGuid()));
            Assert.Equal(System.Net.HttpStatusCode.NotFound, exception.StatusCode);
        }

        [Fact]
        public async Task Update_Should_ThrowNotFound()
        {
            var exception = await Assert.ThrowsAsync<ApiException>(() => _service.Update(new UpdateOrderRequest
            {
                Id = Guid.NewGuid(),
                Value = new OrderDto()
            }));
            Assert.Equal(System.Net.HttpStatusCode.NotFound, exception.StatusCode);
        }

        [Fact]
        public async Task Get_Should_ThrowValidationResult()
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

        [Fact]
        public async Task ShouldThrowNotAcceptable()
        {
            var exception = await Assert.ThrowsAsync<ApiException>(async () => await _service.Baar());
            Assert.NotNull(exception);
            Assert.Equal(HttpStatusCode.NotAcceptable, exception.StatusCode);

            var errorInfo = exception.ToErrorInfo();
            Assert.Equal("Method Baar no allowed", errorInfo.ErrorMessage);
            Assert.Equal((int?)HttpStatusCode.NotAcceptable, errorInfo.ErrorCode);
            Assert.NotNull(errorInfo.StackTrace);
        }


        [Fact]
        public async Task ShouldThrowInternalServerError()
        {
            var exception = await Assert.ThrowsAsync<ApiException>(async () => await _service.Foo());
            Assert.NotNull(exception);
            Assert.Equal(HttpStatusCode.NotImplemented, exception.StatusCode);

            var errorInfo = exception.ToErrorInfo();            

            Assert.Equal((int?)HttpStatusCode.NotImplemented, errorInfo.ErrorCode);
            Assert.NotNull(errorInfo.StackTrace);
            Assert.NotNull(errorInfo.ErrorMessage);
        }

        [Fact]
        public async Task Test()
        {
            var exception = await Assert.ThrowsAsync<ApiException>(async () => await _service.Test());
            Assert.NotNull(exception);
            Assert.Equal(HttpStatusCode.BadRequest, exception.StatusCode);

            var errorInfo = exception.ToErrorInfo() as ValidationResult;
            Assert.NotNull(errorInfo);
            Assert.Equal("Tiene que existir algún análisis especificado", errorInfo.Errors["Test"].First());
        }

        [Fact]
        public async Task ShouldThrowArgumentException()
        {
            var exception = await Assert.ThrowsAsync<ApiException>(async () => await _service.Argument());
            Assert.NotNull(exception);
            Assert.Equal(HttpStatusCode.BadRequest, exception.StatusCode);

            var errorInfo = exception.ToErrorInfo();

            Assert.Equal((int?)HttpStatusCode.BadRequest, errorInfo.ErrorCode);
            Assert.NotNull(errorInfo.StackTrace);
            Assert.NotNull(errorInfo.ErrorMessage);
        }

        [Fact]
        public async Task ShouldUploadImage()
        {
            if (File.Exists("Image.png"))
            {
                File.Delete("Image.png");
            }

            using var fs = File.OpenRead("cybtan.png");

            var result = await _service.UploadImage(new UploadImageRequest
            {
                Size = (int)fs.Length,
                Name = "Image.png",
                Image = fs
            });

            Assert.NotNull(result);
            Assert.NotNull(result.Url);
            Assert.True(File.Exists("Image.png"));
        }

        [Fact]
        public async Task ShouldUploadImageMultipartJson()
        {
            var client = _fixture.CreateClient();

            if (File.Exists("Image2.png"))
            {
                File.Delete("Image2.png");
            }

            using var fs = File.OpenRead("cybtan.png");


            var content = new MultipartFormDataContent("----WebKitFormBoundarymx2fSWqWSd0OxQq1");
            content.Add(new StringContent(JsonConvert.SerializeObject(new
            {
                Size = fs.Length,
                Name = "Image2.png"
            })), "content");

            content.Add(new StreamContent(fs, (int)fs.Length), "Image");

            var response = await client.PostAsync("/api/order/upload", content);

            Assert.NotNull(response);
            Assert.True(response.IsSuccessStatusCode);            
        }

    }
}
