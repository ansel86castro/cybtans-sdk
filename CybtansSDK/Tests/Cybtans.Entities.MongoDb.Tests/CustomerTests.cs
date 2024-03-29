#if INTEGRATIONS

using Cybtans.Entities;
using Cybtans.Entities.MongoDb.Tests.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Cybtans.Entities.MongoDb.Tests
{


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
        public async Task CreateMany()
        {
            await _customers.AddRangeAsync(Enumerable.Range(1,5).Select( i => new Customer
            {
                Name = "CreateMany",
                CreateAt = DateTime.Now,
                State = 1
            }));

            var items = await _customers.ListAll(x => x.Name == "CreateMany");
            Assert.True(items.Count == 5);            
        }


        [Fact]
        public async Task Update()
        {
            var item = await Create("update", 2);
            item.Name = "update test";
            item.State = 3;
            item.UpdateAt = DateTime.UtcNow;

            var update =  await _customers.UpdateAsync(x => x.Id == item.Id, item);

            Assert.Equal(item.Id, update.Id);
            Assert.NotEmpty(update.Id);
            Assert.Equal("update test", update.Name);
            Assert.Equal(3, update.State);
            Assert.NotNull(update.UpdateAt);
            Assert.True(item.CreateAt != new DateTime());

        }

        [Fact]
        public async Task UpdateWithDictionay()
        {
            var item = await Create("update_dic", 2);            
            var update = await _customers.UpdateAsync(x => x.Id == item.Id, new Dictionary<string, object>
            {
                ["Name"] = "update dic 2",
                ["State"] = 3,
                ["UpdateAt"] = DateTime.UtcNow
            });

            Assert.Equal(item.Id, update.Id);
            Assert.NotEmpty(update.Id);
            Assert.Equal("update dic 2", update.Name);
            Assert.Equal(3, update.State);
            Assert.NotNull(update.UpdateAt);
            Assert.True(update.CreateAt != new DateTime());
        }

        [Fact]
        public async Task UpdateWithValues()
        {
            var item = await Create("update_values", 2);
            var update = await _customers.UpdateAsync(x => x.Id == item.Id, new 
            { 
                Name ="update values 2" , 
                State=  3 ,
                UpdateAt = DateTime.UtcNow
            });

            Assert.Equal(item.Id, update.Id);
            Assert.NotEmpty(update.Id);
            Assert.Equal("update values 2", update.Name);
            Assert.Equal(3, update.State);
            Assert.NotNull(update.UpdateAt);
            Assert.True(update.CreateAt != new DateTime());
        }

        [Fact]
        public async Task Delete()
        {
            var item = await Create("delete", 1);            
            var count = await _customers.DeleteAsync(x => x.Id == item.Id);
            Assert.True(count == 1);

            item = await _customers.Get(x => x.Id == item.Id);
            Assert.Null(item);            
        }

        [Fact]
        public async Task ListAll()
        {
            var list = await _customers.ListAll();
            Assert.NotEmpty(list);          
            Assert.All(list, x =>
            {
                Assert.NotEmpty(x.Id);
                Assert.NotEmpty(x.Name);
                Assert.True(x.State > 0);
                Assert.True(x.CreateAt != new DateTime());
            });
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

        [Theory]
        [InlineData(5)]
        [InlineData(50)]
        public async Task GetManyWithFilters(int pageSize)
        {
            int page = 1;
            
            while (true)
            {
                var list = await _customers.GetManyAsync(page, pageSize, 
                    _customers.Filters.Expression(x=>x.State == 1),
                    _customers.Sorting.Ascending(x=>x.Name));

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

        [Fact]        
        public async Task GetManyWithFilters2()
        {
            int page = 1;

            while (true)
            {
                var list = await _customers.GetManyAsync(page, 50,
                    _customers.Filters.And(_customers.Filters.Expression(x => x.State == 2),_customers.Filters.Contains(x=>x.Name, "1")),
                    _customers.Sorting.Ascending(x => x.Name));

                Assert.NotEmpty(list.Items);
                Assert.True(list.Items.Count <= 50);
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

        [Fact]
        public async Task ListAllFilter()
        {
            var list = await _customers.ListAll(
                _customers.Filters.And(
                    _customers.Filters.Expression(x => x.Scoring > 5),
                    _customers.Filters.Expression(x => x.State == 1)
                ),
                _customers.Sorting.Combine(
                    _customers.Sorting.Descending(x => x.Scoring),
                    _customers.Sorting.Ascending(x => x.Name)
                ));

            Assert.NotEmpty(list);
            Assert.All(list, x =>
            {
                Assert.NotEmpty(x.Id);
                Assert.NotEmpty(x.Name);
                Assert.True(x.State > 0);
                Assert.True(x.Scoring > 5);
                Assert.True(x.CreateAt != new DateTime());
            });
        }

        [Fact]
        public async Task ListAllContains()
        {
            var list = await _customers.ListAll(_customers.Filters.Contains(x => x.Name, "1"), null);

            Assert.NotEmpty(list);
            Assert.All(list, x =>
            {
                Assert.NotEmpty(x.Id);
                Assert.NotEmpty(x.Name);
                Assert.Contains("1", x.Name);
                Assert.True(x.CreateAt != new DateTime());
            });
        }

        [Fact]
        public async Task EnumerateAllFilter()
        {

            await using var enumerator = _customers.GetAsyncEnumerator(_customers.Filters.Expression(x=>x.Scoring >= 5), null);
            while (await enumerator.MoveNextAsync())
            {
                var item = enumerator.Current;
                Assert.NotEmpty(item.Id);
                Assert.NotEmpty(item.Name);
                Assert.True(item.Scoring >= 5);
                Assert.True(item.State > 0);
                Assert.True(item.CreateAt != new DateTime());
            }
        }

        [Fact]
        public async Task Querable()
        {
            var query =  _customers.AsQueryable();
            var customers = query
                .Skip(1)
                .Take(2)
                .OrderByDescending(x => x.Scoring)
                .ToList();
            Assert.NotEmpty(customers);
            
        }

        [Fact]
        public async Task ToListLimit()
        {
            var customers = await _customers.ListAll(null, _customers.Sorting.Descending(x=>x.Scoring) , 1, 2);
            Assert.NotEmpty(customers);
        }
    }
}

#endif
