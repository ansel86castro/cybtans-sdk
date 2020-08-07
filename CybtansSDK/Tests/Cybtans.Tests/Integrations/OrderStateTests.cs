using Cybtans.Tests.Clients;
using Cybtans.Tests.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Cybtans.Tests.Integrations
{
    public class OrderStateTests:IClassFixture<IntegrationFixture>
    {
        IntegrationFixture _fixture;
        ITestOutputHelper _testOutputHelper;
        IOrderStateService _service;

        public OrderStateTests(IntegrationFixture fixture, ITestOutputHelper testOutputHelper)
        {
            _fixture = fixture;
            _testOutputHelper = testOutputHelper;
            _service = fixture.GetClient<IOrderStateService>();
        }

        [Fact]
        public async Task CreateOrderState()
        {
            var createOrder = await _service.Create(new OrderStateDto
            {
                 Name = "Test State"
            });

            Assert.NotNull(createOrder);
            Assert.Equal("Test State", createOrder.Name);
        }
    }
}
