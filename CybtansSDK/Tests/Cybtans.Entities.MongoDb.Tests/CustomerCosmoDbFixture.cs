#if INTEGRATIONS

using Cybtans.Entities.MongoDb.Tests.Models;
using Cybtans.Testing.Integration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Cybtans.Entities.MongoDb.Tests
{
    public class CustomerCosmoDbFixture : IAsyncLifetime
    {
        IServiceProvider _serviceProvider;
        ContainerInfo _containerInfo;
        private ServiceCollection _services;

        public CustomerCosmoDbFixture()
        {           
            _services = new ServiceCollection();                    
        }        

        public IObjectRepository<T> GetRepository<T>()
        {
            return _serviceProvider.GetService<IObjectRepository<T>>();
        }

        public async Task InitializeAsync()
        {          
            _services.AddMongoDbProvider<TestMongoDbProvider>(o =>
            {
                o.ConnectionString = "<CONNECTION STRING>";
                o.Database = "test";
            })
             .AddObjectRepositories();

            _serviceProvider = _services.BuildServiceProvider();

            var provider = _serviceProvider.GetService<IMongoClientProvider>();

            var collectionName = provider.GetCollectionFor<Customer>();
            var collection = provider.GetCollection<Customer>();

            if ( (await collection.CountDocumentsAsync(new BsonDocument())) > 0)
            {
                await provider.Database.DropCollectionAsync(collectionName);
            }

            var customer = GetRepository<Customer>();

            await provider.Initialize();

            //var keys = Builders<Customer>.IndexKeys;
            //_ = await collection.Indexes.CreateManyAsync(
            //    new[]
            //    {
            //        new CreateIndexModel<Customer>(keys.Combine(keys.Descending(x=>x.Scoring), keys.Ascending(x=>x.Name))),
            //        new CreateIndexModel<Customer>("{ State: 1}")
            //    });
         

            await customer.AddRangeAsync(Enumerable.Range(1, 10).Select(i => new Customer
            {
                Name = $"Customer {i}",
                CreateAt = DateTime.UtcNow,
                State = i % 2 == 0 ? 1: 2,      
                Scoring = i
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
