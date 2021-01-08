using Cybtans.Entities;
using Cybtans.Testing;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Xunit;
using Xunit.Abstractions;
using System.Linq.Expressions;
using Cybtans.Tests.Domain;

namespace Cybtans.Tests.Entities.EntityFrameworkCore
{

    public class EfRepositoryTests : TestBase<RepositoryFixture>, IDisposable
    {
        IRepository<Order> _repository;
        IServiceScope _scope;

        public EfRepositoryTests(RepositoryFixture fixture, ITestOutputHelper testOutputHelper) : base(fixture, testOutputHelper)
        {
            _scope = fixture.ServiceProvider.CreateScope();
            _repository = GetRepository();
        }

        private IRepository<Order> GetRepository() => _scope.ServiceProvider.GetRequiredService<IRepository<Order>>();

        public void Dispose()
        {
            _scope?.Dispose();
        }


        [Fact]
        public async Task CreateOrder()
        {
            var order = await CreateOrderInternal(_repository);

            var value = await _repository.GetAll(include: new Expression<Func<Order, object>>[]
            {
                    x=>x.Items,
                    x=>x.Customer,
                    x=>x.Customer.CustomerProfile
            })
            .Where(x=>x.Id == order.Id)
            .FirstOrDefaultAsync();

            Assert.NotNull(value);
            Assert.NotNull(value.Customer);
            Assert.NotNull(value.Customer.CustomerProfile);
            Assert.Equal("Test Profile", value.Customer.CustomerProfile.Name);
            Assert.Equal(order.Id, value.Id);
            Assert.Equal(order.CustomerId, value.CustomerId);
            Assert.Equal(1, order.Items.Count);
            
        }

        [Fact]
        public async Task UpdateOrder()
        {
            Order order = await GetOrCreateOrder();

            Assert.NotNull(order);
            Assert.Empty(order.Items);

            order = await _repository.GetAll(include: new Expression<Func<Order, object>>[]
            {
                 x=>x.Items
            }).Where(x => x.Id == order.Id).FirstOrDefaultAsync();

            order.OrderStateId = RepositoryFixture.OrderStateProcessed;
            order.Items.Add(new OrderItem
            {
                ProductName = "Product 2",
                Discount = 0,
                Price = 10
            });

            _repository.Update(order);

            await _repository.UnitOfWork.SaveChangesAsync();

            var value = await _repository.GetAll(include:new Expression<Func<Order, object>>[]{
                x=>x.Items
            }).Where(x => x.Description == "Test Order").FirstOrDefaultAsync();

            Assert.Equal(RepositoryFixture.OrderStateProcessed, order.OrderStateId);
            Assert.Equal(2, order.Items.Count);
        }

        [Fact]
        public async Task DeleteOrder()
        {
            var order = await GetOrCreateOrder();
            _repository.Remove(order);

            await _repository.UnitOfWork.SaveChangesAsync();

            Assert.False(_repository.GetAll().Any(x=>x.Id == order.Id));
        }

        private async Task<Order> GetOrCreateOrder()
        {
            int retry = 2;
            do
            {
                Order order = await _repository.GetAll().Where(x => x.Description == "Test Order").FirstOrDefaultAsync();
                if (order == null)
                {                   
                    await Fixture.Run(async (srv) =>
                    {                        
                        var order = await CreateOrderInternal(srv.GetRequiredService<IRepository<Order>>());
                        Assert.NotNull(order);
                    });                 
                }
                else
                {
                    return order;
                }

                retry--;
            } while (retry > 0);

            return null;
        }

        private static async Task<Order> CreateOrderInternal(IRepository<Order>repository)
        {
            var order = new Order
            {
                OrderStateId = 1,
                CustomerId = RepositoryFixture.CustomerId,
                Description = "Test Order",
                Items =
                {
                    new OrderItem
                    {
                         ProductName = "Product 1",
                         Discount = 10,
                         Price = 10
                    }
                }
            };

            repository.Add(order);

            await repository.UnitOfWork.SaveChangesAsync();

            return order;
        }
    }
}
