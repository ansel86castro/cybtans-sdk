using Cybtans.Tests.Models;
using Moq;
using System.Net;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using Cybtans.Common;
using Cybtans.Tests.Services;

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

        [Fact]
        public async Task GetOrderStates()
        {
            var result = await _service.GetAll(new GetAllRequest { });
            Assert.NotNull(result);
            Assert.NotEmpty(result.Items);
            Assert.True(result.TotalCount > 0);
            Assert.Equal(result.TotalCount, result.Items.Count);
        }

        [Fact]
        public async Task GetOrderStatesWithFilter()
        {
            var result = await _service.GetAll(new() { Filter = "name eq 'Submitted'" });
            Assert.NotNull(result);
            Assert.NotEmpty(result.Items);
            Assert.True(result.TotalCount == 1);
            Assert.Equal(result.TotalCount, result.Items.Count);
        }

        [Fact]
        public async Task GetOrderStatesWithNoRole()
        {
            await _fixture.CreateTest()
                .UseRoles("no-admin")
                .RunAsync<IOrderStateService>(async service =>
                {
                    var exception = await Assert.ThrowsAsync<ApiException>(() => service.GetAll(new Models.GetAllRequest { }));
                    Assert.NotNull(exception);
                    Assert.Equal(HttpStatusCode.Forbidden, exception.StatusCode);
                });
        }

        [Fact]
        public async Task GetOrderStatesMockService()
        {
            var mockService = new Mock<Services.IOrderStateService>();
            mockService.Setup(x => x.Get(It.Is<GetOrderStateRequest>(x=>x.Id == 1)))
            .ReturnsAsync(new OrderStateDto
            {
                 Id = 1,
                 Name = "Mock Test Sample"
            });

            mockService.Setup(x => x.Get(It.Is<GetOrderStateRequest>(x => x.Id == 2)))
                .ReturnsAsync((OrderStateDto)null);

            await _fixture.CreateTest()
                .UseService(mockService.Object)
                .RunAsync<IOrderStateService>(async service =>
                {
                    var result = await service.Get(1);
                    Assert.NotNull(result);
                    Assert.Equal("Mock Test Sample", result.Name);

                    result = await service.Get(2);
                    Assert.Null(result);

                });
        }
    }
}
