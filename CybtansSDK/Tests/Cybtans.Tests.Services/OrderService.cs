
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

namespace Cybtans.Tests.Services
{
    [RegisterDependency(typeof(IOrderService))]
    public class OrderService : CrudService<Order, Guid, OrderDto, GetOrderRequest, GetAllRequest, GetAllOrderResponse, UpdateOrderRequest, CreateOrderRequest, DeleteOrderRequest>, 
        IOrderService
    {
        private IBroadcastService _broadCastService;
        private readonly Greeter.GreeterClient _greeterClient;

        public OrderService(IRepository<Order, Guid> repository, IUnitOfWork uow, IMapper mapper, ILogger<OrderService> logger,
            IBroadcastService broadCastService, Cybtans.Tests.Grpc.Greeter.GreeterClient greeterClient)
            : base(repository, uow, mapper, logger) 
        {
            _broadCastService = broadCastService;
            _greeterClient = greeterClient;
        }

        public async Task<CustomerDto> SayHelloAsync(OrderDto request)
        {            
            try
            {
               Logger.LogDebug("Grpc call Cybtans.Tests.Grpc.Greeter.Greeter.SayHelloAsync");

               var response = await _greeterClient.SayHelloAsync(Mapper.Map<OrderDto, Cybtans.Tests.Grpc.HelloRequest>(request));
               return Mapper.Map<Cybtans.Tests.Grpc.HelloReply, CustomerDto>(response);
            }
            catch(RpcException e)
            {
                Logger.LogError(e, "Grpc call failed Cybtans.Tests.Grpc.Greeter.Greeter.SayHelloAsync");
                throw;
            }
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

                request.Image.Position = 0;
                var hash = await Task.Run(() => new SymetricCryptoService().ComputeHash(request.Image));
                var checkSum = CryptoService.ToStringX2(hash);

                return new UploadImageResponse
                {
                    Url = "http://localhost/image.jpg",
                    M5checksum = checkSum
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
            var hash = await Task.Run(() => new SymetricCryptoService().ComputeHash(request.Data));

            return CryptoService.ToStringX2(hash);
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
    }
}