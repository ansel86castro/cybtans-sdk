using AutoMapper;
using Cybtans.Test.Domain;
using Cybtans.Test.Domain.EF;
using Cybtans.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace Cybtans.Tests.Entities.EntityFrameworkCore
{
    public class RepositoryFixture: BaseFixture
    {
        public static readonly Guid CustomerId = Guid.NewGuid();
        public static readonly Guid CustomerProfileId = Guid.NewGuid();

        public static readonly int OrderStateDraft = 1;
        public static readonly int OrderStateSubmitted = 2;
        public static readonly int OrderStateProcessed = 3;
        public static readonly int OrderStateShipped = 4;


        ServiceCollection _services;
        IServiceProvider _serviceProvider;
        DbConnection _dbConnection;

        public RepositoryFixture()
        {
            _dbConnection = AdventureContext.CreateInMemoryDatabase();

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
                await context.Database.EnsureCreatedAsync();
                await Seed(context);
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
            builder.UseSqlite(connection ?? AdventureContext.CreateInMemoryDatabase());
            builder.EnableSensitiveDataLogging(true);

            return new AdventureContext(builder.Options);
        }

      

        public IServiceProvider ServiceProvider => _serviceProvider ?? (_serviceProvider = _services.BuildServiceProvider());

        public async Task Run(Func<IServiceProvider, Task> action)
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                await action(scope.ServiceProvider);
            }
        }

        public async Task<T> Run<T>(Func<IServiceProvider, Task<T>> action)
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                return await action(scope.ServiceProvider);
            }
        }

        public static async Task Seed(AdventureContext context)
        {
            if (!context.OrderStates.Any())
            {
                context.OrderStates.AddRange(
                    new OrderState { Id = 1, Name = "Draft" },
                    new OrderState { Id = 2, Name = "Submitted" },
                    new OrderState { Id = 3, Name = "Processed" },
                    new OrderState { Id = 4, Name = "Shipped" },
                    new OrderState { Id = 5, Name = "Delivered" });

                context.Add(new Customer
                {
                    Id = CustomerId,
                    Name = "Test",
                    FirstLastName = "Test",
                    SecondLastName = "Test",
                    CustomerProfile = new CustomerProfile
                    {
                        Id = CustomerProfileId,
                        Name = "Test Profile"
                    }
                });

                await context.SaveChangesAsync();
            }
        }
    }
}
