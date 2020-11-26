using Microsoft.Extensions.DependencyInjection;
using System;
using Cybtans.Services;
using Cybtans.Services.Extensions;
using System.Threading.Tasks;
using Xunit;
using System.Collections.Generic;
using System.Linq;
using Cybtans.Services.Locking;
using Microsoft.Extensions.Logging;
using Cybtans.Services.Caching;

namespace Cybtans.Redis.Tests
{
    class Program
    {
        static async Task Main(string[] args)
        {
            IServiceCollection services = new ServiceCollection();
                        
            services.AddLogging(builder=>
            {
                builder.AddConsole();   
            });

            services.AddRedisCache(o => o.Connection = "localhost");
            services.AddDistributedLockProvider();

            using (var sp = services.BuildServiceProvider())
            {              
                await Task.WhenAll(
                    TestLock(sp.GetService<ILockProvider>()),
                    TestSet(sp.GetService<ICacheService>()) 
                );                
            }
        }
     
        public static async Task TestLock(ILockProvider lockProvider)
        {
            var keys = new string[] { "BBFCC95A-2FA1-45DA-A289-FF486C33AFC9", "56FCC95A-2FA1-45DA-A289-FF486C33AFC8" };
            var data = new Dictionary<string, List<int>>(keys.Select(x => new KeyValuePair<string, List<int>>(x, new List<int>())));

            Random ran = new Random();

            await Task.WhenAll(Enumerable.Range(0, 4).Select(async i =>
            {
                var key = keys[i % keys.Length];

                using (var @lock = await lockProvider.GetLock(key))
                {
                    await Task.Delay(ran.Next(100, 300));                    

                    for (int n = 0; n < 10; n++)
                    {
                        data[key].Add(10 * i + n);
                    }
                }
            }));

            foreach (var kv in data)
            {
                var sorted = kv.Value.OrderBy(x => x).ToList();
                Assert.Equal(sorted, kv.Value);
            }
        }
    
    
        public static async Task TestSet(ICacheService cache)
        {
            await cache.Set("STORE:1", new Store
            {
                Name = "Item",
                Date = DateTime.Now,
                Code = 1
            }, TimeSpan.FromSeconds(5));

            var store = await cache.Get<Store>("STORE:1");
            Assert.NotNull(store);
            Assert.Equal("Item", store.Name);
            Assert.Equal(1, store.Code);

            var result = await cache.GetOrSet<Store>("STORE:2", () => Task.FromResult(new Store
            {
                Name = "Item",
                Date = DateTime.Now,
                Code = 1
            }), TimeSpan.FromSeconds(5));

            Assert.NotNull(result);

            var l = await cache.Increment("INC:1", 2, expire: TimeSpan.FromSeconds(1));
            Assert.Equal(2, l);

            await Task.Delay(2000);

            var value = await cache.GetInteger("INC:1");
            Assert.Null(value);

            l = await cache.Increment("INC:1", 2);
            await Task.Delay(2000);

            value = await cache.GetInteger("INC:1");
            Assert.NotNull(value);
        }
    }

    public class Store
    {
        public string Name { get; set; }

        public DateTime Date { get; set; }

        public int Code { get; set; }
    }
}
