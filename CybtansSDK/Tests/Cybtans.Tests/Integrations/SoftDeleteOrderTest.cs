﻿using Cybtans.Clients;
using Cybtans.Tests.Models;
using Cybtans.Tests.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using Cybtans.Common;

namespace Cybtans.Tests.Integrations
{
   public class SoftDeleteOrderTest : IClassFixture<IntegrationFixture>
    {
        IntegrationFixture _fixture;
        ITestOutputHelper _testOutputHelper;
        ISoftDeleteOrderService _service;

        public SoftDeleteOrderTest(IntegrationFixture fixture, ITestOutputHelper testOutputHelper)
        {
            _fixture = fixture;
            _testOutputHelper = testOutputHelper;
            _service = fixture.GetClient<ISoftDeleteOrderService>();
        }

        private async Task<SoftDeleteOrderDto> CreateOrderInternal()
        {
            var order = new SoftDeleteOrderDto
            {               
                Name = "Test Order",
                CreateDate= DateTime.Now,                 
                Items = new List<SoftDeleteOrderItemDto>
                    {
                        new SoftDeleteOrderItemDto
                        {
                             Name = "SoftDeleteProduct 1",
                              CreateDate = DateTime.Now,                               
                        }
                    }
            };

            return await _service.Create(order);
        }

        [Fact]
        public async Task AfterSoftDeleteOrder_ShouldNotDelete()
        {
            var order = await CreateOrderInternal();          
            await _service.Delete(order.Id);

            var exception = await Assert.ThrowsAsync<ApiException>(() => _service.Delete(order.Id));
            Assert.Equal(System.Net.HttpStatusCode.NotFound, exception.StatusCode);
        }

        [Fact]
        public async Task AfterSoftDeleteOrder_ShouldNotGet()
        {
            var order = await CreateOrderInternal();
            await _service.Delete(order.Id);

            var exception = await Assert.ThrowsAsync<ApiException>(() => _service.Get(order.Id));
            Assert.Equal(System.Net.HttpStatusCode.NotFound, exception.StatusCode);
        }


        [Fact]
        public async Task AfterSoftDeleteOrder_ShouldShownInGetAll()
        {
            var order = await CreateOrderInternal();
            await _service.Delete(order.Id);

            var result = await _service.GetAll(new Models.GetAllRequest { }) ;

            Assert.DoesNotContain(result.Items, x => x.Id == order.Id);
          
        }

        [Fact]
        public async Task AfterSoftDeleteOrder_ShouldNotUpdate()
        {
            var order = await CreateOrderInternal();
            await _service.Delete(order.Id);

            var exception = await Assert.ThrowsAsync<ApiException>(() => _service.Update(new UpdateSoftDeleteOrderRequest
            {
                Id = order.Id,
                Value = order
            }));

            Assert.Equal(System.Net.HttpStatusCode.NotFound, exception.StatusCode);
        }
    }
}
