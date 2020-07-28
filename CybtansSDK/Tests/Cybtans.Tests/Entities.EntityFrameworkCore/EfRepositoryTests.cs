using Cybtans.Entities;
using Cybtans.Test.Domain;
using Cybtans.Test.Domain.EF;
using Cybtans.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Data.Common;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Cybtans.Tests.Entities.EntityFrameworkCore
{
    public class RepositoryFixture: BaseFixture
    {
        ServiceCollection _services;
        IServiceProvider _serviceProvider;
        DbConnection _dbConnection;

        public RepositoryFixture()
        {
            _dbConnection = CreateInMemoryDatabase();

            _services = new ServiceCollection();
            
            _services.AddDbContext<AdventureContext>(builder=>
                builder.UseSqlite(_dbConnection));

            _services.AddUnitOfWork<AdventureContext>()
                .AddRepositories();            
        }

        public override Task InitializeAsync()
        {
            return Run(async provider =>
            {
                var context = provider.GetService<AdventureContext>();
                await context.Database.MigrateAsync();
            });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {                
                _dbConnection.Close();                
            }
            base.Dispose(disposing);
        }

        public static AdventureContext CreateContext(DbConnection connection = null)
        {
            var builder = new DbContextOptionsBuilder<AdventureContext>();
            builder.UseSqlite(connection ?? CreateInMemoryDatabase());
            return new AdventureContext(builder.Options);
        }

        public static DbConnection CreateInMemoryDatabase()
        {
            var connection = new SqliteConnection("Data Source=AdventureWorks;Mode=Memory;Cache=Shared");
            connection.Open();

            return connection;
        }

        public IServiceProvider ServiceProvider => _serviceProvider ?? (_serviceProvider = _services.BuildServiceProvider());

        public async Task Run(Func<IServiceProvider, Task> action)
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                await action(scope.ServiceProvider);
            }
        }
    }

    public class EfRepositoryTests : TestBase<RepositoryFixture>, IDisposable
    {
        IRepository<Order> _repository;
        IServiceScope _scope;

        public EfRepositoryTests(RepositoryFixture fixture, ITestOutputHelper testOutputHelper) : base(fixture, testOutputHelper)
        {
            _scope = fixture.ServiceProvider.CreateScope();
            _repository = _scope.ServiceProvider.GetRequiredService<IRepository<Order>>();
        }

        public void Dispose()
        {
            _scope?.Dispose();
        }
        

        [Fact]
        public async Task CreateProduct()
        {
          
        }
    }
}
