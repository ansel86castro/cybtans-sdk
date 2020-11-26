using Cybtans.Services.Locking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace Cybtans.Tests.Services
{
    public class MemoryLockProviderTest
    {
        MemoryLockProvider _lockProvider;

        public MemoryLockProviderTest()
        {
            _lockProvider = new MemoryLockProvider();
        }

        [Fact]
        public async Task TestLock()
        {
            var keys = new string[] { "BBFCC95A-2FA1-45DA-A289-FF486C33AFC9", "56FCC95A-2FA1-45DA-A289-FF486C33AFC8" };
            var data = new Dictionary<string, List<int>>(keys.Select(x=> new KeyValuePair<string, List<int>>(x, new List<int>())));

            Random ran = new Random();

            await Task.WhenAll(Enumerable.Range(0, 20).Select(async i =>
            {
                var key = keys[i % keys.Length];                

                using(var @lock = await _lockProvider.GetLock(key))
                {
                    await Task.Delay(ran.Next(10, 100));

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
    }
}
