
using System;
using AutoMapper;
using Cybtans.Entities;
using Cybtans.Services;
using Microsoft.Extensions.Logging;
using Cybtans.Tests.Models;
using System.Threading.Tasks;
using System.Net;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics.Contracts;
using Cybtans.Services.Security;
using Cybtans.Tests.Domain;
using Cybtans.Messaging;
using AutoMapper.QueryableExtensions;
using Cybtans.Tests.Grpc;
using Grpc.Core;
using SQLitePCL;
using System.Linq;

namespace Cybtans.Tests.Services
{
    [RegisterDependency(typeof(IOrderService))]
    public class OrderService : CrudService<Order, Guid, OrderDto, GetOrderRequest, GetAllRequest, GetAllOrderResponse, UpdateOrderRequest, CreateOrderRequest, DeleteOrderRequest>, 
        IOrderService
    {
        private IBroadcastService _broadCastService;
        private readonly IDatabaseConnectionFactory _connectionFactory;

        public OrderService(
            IRepository<Order, Guid> repository, 
            IUnitOfWork uow, 
            IMapper mapper, 
            ILogger<OrderService> logger,
            IBroadcastService broadCastService,
            IDatabaseConnectionFactory connectionFactory)
            : base(repository, uow, mapper, logger) 
        {
            _broadCastService = broadCastService;
            _connectionFactory = connectionFactory;
        }     

        public Task Argument()
        {
            throw new ArgumentException("Invalid Argument", "arg");
        }

        public Task Baar()
        {
            throw new CybtansException(HttpStatusCode.NotAcceptable, "Method Baar no allowed");
        }
       

        public Task Foo()
        {
            throw new NotImplementedException();
        }

        public async Task Test()
        {
            await ValidateTest();
        }

        public async Task<UploadImageResponse> UploadImage(UploadImageRequest request)
        {
            if (request.Image == null)
            {
                throw new ValidationException().AddError("Image", "Image is required");
            }
            try
            {
                using (var fs = new FileStream(request.Name, FileMode.Create, FileAccess.Write))
                {
                    await request.Image.CopyToAsync(fs);

                    await fs.FlushAsync();
                }

                using var imageStream = File.OpenRead(request.Name);
                var hash = await Task.Run(() => new SymetricCryptoService().ComputeHash(imageStream));
                var checkSum = CryptoService.ToStringX2(hash);

                return new UploadImageResponse
                {
                    Url = "http://localhost/image.jpg",
                    M5Checksum = checkSum
                };
            }
            catch (Exception e)
            {
                Logger.LogError(e, e.Message);
                throw;
            }
        }

        public async Task<UploadStreamResponse> UploadStream(Stream stream)
        {                                  
            var hash =new SymetricCryptoService().ComputeHash(stream);
            var checkSum = CryptoService.ToStringX2(hash);
            return checkSum;
        }

        public async Task<UploadStreamResponse> UploadStreamById(UploadStreamByIdRequest request)
        {
            var memStream = new MemoryStream();
            await request.Data.CopyToAsync(memStream);
            memStream.Position = 0;
            return CryptoService.ToStringX2(new SymetricCryptoService().ComputeHash(memStream));
        }

        public Task<DowndloadImageResponse> DownloadImage(DownloadImageRequest request)
        {
            Stream stream = File.OpenRead("moon.jpg");
            return Task.FromResult(new DowndloadImageResponse
            {
                FileName = "moon.jpg",
                ContentType = "image/jpg",
                Image = stream
            });
        }

        private async Task ValidateTest()
        {
            await Task.Delay(10);

            throw new ValidationException("Error Test")
                .AddError("Test", "Tiene que existir algún análisis especificado");
        }

        public Task GetMultiPath(MultiPathRequest request)
        {
            return Task.CompletedTask;
        }

        public async Task SendNotification(OrderNotification request)
        {
            await _broadCastService.Publish(request, "Orders");
        }

        public async Task<GetAllNamesResponse> GetAllNames()
        {
            using var conn = _connectionFactory.GetConnection();
            var items = await conn.QueryAsync<OrderNamesDto>("SELECT Id, Description FROM orders");

            return new GetAllNamesResponse
            {
                Items = items.ToList()
            };
        }

        public async Task<OrderNamesDto> GetOrderName(GetOrderNameRequest request)
        {
            using var conn = _connectionFactory.GetConnection();
            var item = await conn.QueryFirstOrDefaultAsync<OrderNamesDto>("SELECT Id, Description FROM orders WHERE Id=@Id", 
                new { request.Id });

            return item;
        }

        public async Task<OrderNamesDto> CreateOrderName(CreateOrderNameRequest request)
        {
            using var conn = _connectionFactory.GetConnection();
            var id = Guid.NewGuid();

            var customer = await conn.QueryFirstOrDefaultAsync<CustomerName>("SELECT * FROM customers LIMIT 1");

            var rows = await conn.ExecuteAsync(@"INSERT INTO orders(
                                            Id, 
                                            Description, 
                                            CustomerId, 
                                            OrderStateId,
                                            OrderType,
                                            TenantId,
                                            CreateDate,
                                            UpdateDate,
                                            Creator
                                            ) VALUES(@Id, @Description, @CustomerId, @OrderStateId, @OrderType, 0, date('now'), NULL, NULL)",
                                            new
                                            {
                                                Id = id.ToString(),
                                                Description =  request.Name,
                                                CustomerId = customer.Id,
                                                OrderStateId = 1,
                                                OrderType = Domain.OrderTypeEnum.Normal,
                                            });
            if (rows <= 0)
                throw new InvalidOperationException();
            return await GetOrderName(id.ToString());          
        }
    }
}