using Cybtans.Services.Caching;
using System;
using System.Threading.Tasks;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using Cybtans.Tests.Domain;

namespace Cybtans.Tests.Integrations
{
    public class LocalCacheTest : IClassFixture<IntegrationFixture>
    {
        private readonly IntegrationFixture _fixture;
        private ICacheService _cacheService;

        public LocalCacheTest(IntegrationFixture fixture)
        {
            _fixture = fixture;
            _cacheService = fixture.Services.GetService<ICacheService>();
        }

        [Fact]
        public async Task GetSet()
        {
            var customer = new Customer { Id = Guid.NewGuid(), Name = "Customer" };
            await _cacheService.Set(customer.Id.ToString(), customer);

            var result = await _cacheService.Get<Customer>(customer.Id.ToString());

            Assert.NotNull(result);
            Assert.Equal(customer.Id, result.Id);
            Assert.Equal(customer.Name, result.Name);
        }

        [Fact]
        public async Task Increment()
        {
            await _cacheService.Increment("inc1");

            var counter =  await _cacheService.GetInteger("inc1");
            Assert.Equal(1, counter);

            await _cacheService.Decrement("inc1");

            counter = await _cacheService.GetInteger("inc1");
            Assert.Equal(0, counter);
        }
    }
}
