using Cybtans.AspNetCore;
using Cybtans.Refit;
using Cybtans.Services;
using Cybtans.Tests.Entities.EntityFrameworkCore;
using Cybtans.Tests.Models;
using Cybtans.Tests.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using AutoMapper;
using System.IO;
using Newtonsoft.Json;
using System.Text;
using Cybtans.Serialization;
using System.Net.Http.Headers;
using Cybtans.Services.Security;
using Cybtans.Tests.Domain.EF;
using Cybtans.Tests.Domain;
using Cybtans.Entities.Extensions;

namespace Cybtans.Tests.Integrations
{
    public class OrdersTests: IClassFixture<IntegrationFixture>
    {
        IntegrationFixture _fixture;
        ITestOutputHelper _testOutputHelper;        
        Clients.IOrderServiceClient _service;

        public OrdersTests(IntegrationFixture fixture, ITestOutputHelper testOutputHelper)
        {
            _fixture = fixture;
            _testOutputHelper = testOutputHelper;
            _service = fixture.GetClient<Clients.IOrderServiceClient>();
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
        public async Task GetAll()
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
            Assert.Equal("Description can not be empty", result.Errors["Value.Description"].First());
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

            UploadImageResponse result = await _service.UploadImage(new UploadImageRequest
            {
                Size = (int)fs.Length,
                Name = "Image.png",
                Image = fs
            });

            Assert.NotNull(result);
            Assert.NotNull(result.Url);
            Assert.True(File.Exists("Image.png"));

            fs.Seek(0, SeekOrigin.Begin);
            var hash = CryptoService.ToStringX2(new SymetricCryptoService().ComputeHash(fs));

            Assert.Equal(hash, result.M5checksum);
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
            var json = JsonConvert.SerializeObject(new { Size = fs.Length, Name = "Image2.png" });

            content.Add(new StringContent(json, Encoding.UTF8, "application/json") , "content");
            content.Add(new StreamContent(fs, (int)fs.Length), "Image", "Image.png");

            var response = await client.PostAsync("/api/order/upload", content);

            Assert.NotNull(response);
            Assert.True(response.IsSuccessStatusCode);

            var bytes = await response.Content.ReadAsByteArrayAsync();
            UploadImageResponse obj = BinaryConvert.Deserialize<UploadImageResponse>(bytes);
            fs.Seek(0, SeekOrigin.Begin);
            var hash = CryptoService.ToStringX2(new SymetricCryptoService().ComputeHash(fs));

            Assert.Equal(hash, obj.M5checksum);
        }

        [Fact]
        public async Task ShouldUploadImageMultipartForm()
        {
            var client = _fixture.CreateClient();

            if (File.Exists("Image2.png"))
            {
                File.Delete("Image2.png");
            }

            using var fs = File.OpenRead("cybtan.png");


            var content = new MultipartFormDataContent("----WebKitFormBoundarymx2fSWqWSd0OxQq1");

            var form = $"Size={fs.Length}&Name=Image2.png";

            content.Add(new StringContent(form, Encoding.UTF8), "content");
            content.Add(new StreamContent(fs, (int)fs.Length), "Image", "Image.png");

            var response = await client.PostAsync("/api/order/upload", content);

            Assert.NotNull(response);
            Assert.True(response.IsSuccessStatusCode);
        }

        [Fact]
        public async Task ShouldUploadImageMultipartBinary()
        {
            var client = _fixture.CreateClient();

            if (File.Exists("Image2.png"))
            {
                File.Delete("Image2.png");
            }

            using var fs = File.OpenRead("cybtan.png");


            var content = new MultipartFormDataContent("----WebKitFormBoundarymx2fSWqWSd0OxQq1");

            var bytes = BinaryConvert.Serialize(new { Size = fs.Length, Name = "Image2.png" });

            var byteArrayContent = new ByteArrayContent(bytes);
            byteArrayContent.Headers.ContentType = MediaTypeHeaderValue.Parse($"{BinarySerializer.MEDIA_TYPE}; charset={BinarySerializer.DefaultEncoding.WebName}");
           
            content.Add(byteArrayContent);
            content.Add(new StreamContent(fs, (int)fs.Length), "Image", "Image.png");

            var response = await client.PostAsync("/api/order/upload", content);

            Assert.NotNull(response);
            Assert.True(response.IsSuccessStatusCode);
        }

        [Fact]
        public async Task ShouldUploadStreamById()
        {        
            using var fs = File.OpenRead("cybtan.png");
            var result = await _service.UploadStreamById(new UploadStreamByIdRequest
            {
                Id = Guid.NewGuid().ToString(),
                Data = fs
            });

            Assert.NotNull(result);

            fs.Seek(0, SeekOrigin.Begin);
            var hash = CryptoService.ToStringX2(new SymetricCryptoService().ComputeHash(fs));

            Assert.NotNull(result.M5checksum);
            Assert.Equal(hash, result.M5checksum);
        }

        [Fact]
        public async Task ShouldDownloadImage()
        {                     
            var result = await _service.DownloadImage("Image.png");

            Assert.NotNull(result);
            Assert.NotNull(result.Image);
            Assert.Equal("moon.jpg", result.FileName);
            Assert.Equal("image/jpg", result.ContentType);
        
            using var fs = File.OpenRead("moon.jpg");
            var targetHash = CryptoService.ToStringX2(new SymetricCryptoService().ComputeHash(fs));
            var destHash = CryptoService.ToStringX2(new SymetricCryptoService().ComputeHash(result.Image));

            Assert.Equal(targetHash, destHash);
        }

        [Fact]
        public async Task GetAllFiltered()
        {
            var result = await _service.GetAll(new GetAllRequest
            {
                Filter = "customer.name = 'Test' and customer.customerProfile.name = 'Test Profile'"
            });

            Assert.NotNull(result);
            Assert.True(result.TotalCount > 0);
            Assert.NotEmpty(result.Items);
        }

        [Fact]
        public async Task GetAllFilteredWithFunction()
        {
            var result = await _service.GetAll(new GetAllRequest
            {
                Filter = "items.any(price = 10)"
            });

            Assert.NotNull(result);
            Assert.True(result.TotalCount > 0);
            Assert.NotEmpty(result.Items);
        }

        [Fact]
        public async Task GetAllFilterByDiscount()
        {
            var result = await _service.GetAll(new GetAllRequest
            {
                Filter = "items.any(discount = 0.5)"
            });

            Assert.True(result.TotalCount > 0);
            Assert.NotEmpty(result.Items);

            result = await _service.GetAll(new GetAllRequest
            {
                Filter = "items.any(discount = 5)"
            });

            Assert.True(result.TotalCount > 0);
            Assert.NotEmpty(result.Items);

            result = await _service.GetAll(new GetAllRequest
            {
                Filter = "items.any(discount = null)"
            });

            Assert.True(result.TotalCount > 0);
            Assert.NotEmpty(result.Items);
        }


        [Fact]
        public async Task GetAllNames()
        {
            var result = await _service.GetAllNames();
          
            Assert.NotNull(result);
            Assert.NotEmpty(result.Items);
        }

        [Fact]
        public async Task GetOrderName()
        {
            var orders = await _service.GetAllNames();

             var result = await _service.GetOrderName(new GetOrderNameRequest
            {
                Id = orders.Items.First().Id
            });

            Assert.NotNull(result);
        }

        [Fact(Skip = "Skip")]
        public async Task CreateOrderName()
        {
            var result = await _service.CreateOrderName(new CreateOrderNameRequest
            {
                 Name = "CreateOrderName"
            });
            
            Assert.NotNull(result);
        }


        //[Fact]
        //public async Task ShouldUploadStream()
        //{
        //    FileStream fs = null;
        //    try
        //    {
        //        fs = File.OpenRead("cybtan.png");
        //        var result = await _service.UploadStream(fs);

        //        Assert.NotNull(result);

        //        if (!fs.CanRead)
        //        {
        //            fs = File.OpenRead("cybtan.png");
        //        }
        //        var hash = CryptoService.ToStringX2(new SymetricCryptoService().ComputeHash(fs));

        //        Assert.NotNull(result.M5checksum);
        //        Assert.Equal(hash, result.M5checksum);
        //    }
        //    finally
        //    {
        //        fs?.Close();
        //    }
        //}

    }
}
