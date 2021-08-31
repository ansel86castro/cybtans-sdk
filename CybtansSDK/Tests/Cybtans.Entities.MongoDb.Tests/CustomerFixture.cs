#if !INTEGRATIONS

using Cybtans.Entities.MongoDb.Tests.Models;
using Cybtans.Testing.Integration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Cybtans.Entities.MongoDb.Tests
{
    public class CustomerFixture : IAsyncLifetime
    {
        IServiceProvider _serviceProvider;
        ContainerInfo _containerInfo;
        private ServiceCollection _services;

        public CustomerFixture()
        {           
            _services = new ServiceCollection();         
           
        }        

        public IObjectRepository<T> GetRepository<T>()
        {
            return _serviceProvider.GetService<IObjectRepository<T>>();
        }

        public async Task InitializeAsync()
        {
            var docker = new DockerManager();
            var config = new MongoDbContainerConfig(port: 0);
            _containerInfo = await docker.RunContainerAsync(config);
            Assert.NotNull(_containerInfo);

            _services.AddMongoDbProvider<TestMongoDbProvider>(o =>
            {
                o.ConnectionString =  config.GetConnectionString(_containerInfo);
                o.Database = "test";
            })
             .AddObjectRepositories();

            _serviceProvider = _services.BuildServiceProvider();

            var customer = GetRepository<Customer>();
            await customer.AddRangeAsync(Enumerable.Range(1, 10).Select(i => new Customer
            {
                Name = $"Customer {i}",
                CreateAt = DateTime.UtcNow,
                State = i % 2 == 0 ? 1: 2
            }));
        }

        public async Task DisposeAsync()
        {
            if (_containerInfo != null)
            {
                await _containerInfo.DisposeAsync();
            }
        }
    }
}

#endif
