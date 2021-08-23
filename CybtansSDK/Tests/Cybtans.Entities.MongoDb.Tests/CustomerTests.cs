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
                CreateAt = DateTime.Now,
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

    
    public class CustomerTests : IClassFixture<CustomerFixture>
    {
        IObjectRepository<Customer> _customers;
        public CustomerTests(CustomerFixture fixture)
        {
            _customers = fixture.GetRepository<Customer>();
        }

        [Fact]
        public async Task EnumerateAll()
        {
            var list = await _customers.ToListAsync();
            Assert.NotEmpty(list);
            Assert.Equal(10, list.Count);
            Assert.All(list, x =>
            {
                Assert.NotEmpty(x.Id);
                Assert.NotEmpty(x.Name);
                Assert.True(x.State > 0);
                Assert.True(x.CreateAt != new DateTime());
            });
        }

        [Fact]
        public async Task GetAsyncEnumerator()
        {
            await using var enumerator  =  _customers.GetAsyncEnumerator();
            while(await enumerator.MoveNextAsync())
            {
                var item = enumerator.Current;
                Assert.NotEmpty(item.Id);
                Assert.NotEmpty(item.Name);
                Assert.True(item.State > 0);
                Assert.True(item.CreateAt != new DateTime());
            }
            
        }

        [Theory]
        [InlineData("create", 1)]
        public async Task<Customer> Create(string name ,int state)
        {
           var item = await _customers.AddAsync(new Customer
            {
                Name = name,
                CreateAt = DateTime.Now,
                State = state
            });

            Assert.NotEmpty(item.Id);
            Assert.Equal(name, item.Name);
            Assert.Equal(state, item.State);
            Assert.True(item.CreateAt != new DateTime());

            return item;
        }

        [Fact]
        public async Task Update()
        {
            var item = await Create("update", 2);
            item.Name = "update test";
            item.State = 3;
            await _customers.UpdateAsync(x => x.Id == item.Id, item);

             item = await _customers.Get(x=> x.Id == item.Id);
            Assert.NotEmpty(item.Id);
            Assert.Equal("update test", item.Name);
            Assert.Equal(3, item.State);
        }

        [Fact]
        public async Task Delete()
        {
            var item = await Create("delete", 1);            
            await _customers.DeleteAsync(x => x.Id == item.Id);

            item = await _customers.Get(x => x.Id == item.Id);
            Assert.Null(item);            
        }

        [Theory]
        [InlineData(5)]
        [InlineData(50)]
        public async Task GetMany(int pageSize)
        {
            int page = 1;
            
            while (true)
            {
                var list = await _customers.GetManyAsync(page, pageSize);

                Assert.NotEmpty(list.Items);
                Assert.True(list.Items.Count <= pageSize);
                Assert.All(list.Items, x =>
                {
                    Assert.NotEmpty(x.Id);
                    Assert.NotEmpty(x.Name);
                    Assert.True(x.State > 0);
                    Assert.True(x.CreateAt != new DateTime());
                });

                if (page >= list.TotalPages)
                    break;

                page++;
            }
        }
    }
}

#endif
