
using System;
using AutoMapper;
using Cybtans.Entities;
using Cybtans.Services;
using Microsoft.Extensions.Logging;
using Cybtans.Test.Domain;
using Cybtans.Tests.Models;
using System.Threading.Tasks;
using System.Net;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics.Contracts;
using Cybtans.Services.Security;

namespace Cybtans.Tests.Services
{
    [RegisterDependency(typeof(IOrderService))]
    public class OrderService : CrudService<Order, Guid, OrderDto, GetOrderRequest, GetAllRequest, GetAllOrderResponse, UpdateOrderRequest, DeleteOrderRequest>, 
        IOrderService
    {
        public OrderService(IRepository<Order, Guid> repository, IUnitOfWork uow, IMapper mapper, ILogger<OrderService> logger)
            : base(repository, uow, mapper, logger) { }

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
    }
}