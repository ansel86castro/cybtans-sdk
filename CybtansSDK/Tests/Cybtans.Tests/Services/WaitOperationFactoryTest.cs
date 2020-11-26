using Cybtans.Services.Utils;
using Cybtans.Tests.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Cybtans.Tests.Services
{
    public class WaitOperationFactoryTest
    {
        private WaitOperationManager _factory;

        public WaitOperationFactoryTest()
        {
            _factory = new WaitOperationManager();
        }

        [Fact]
        public async Task WaitForResult()
        {
            var op = _factory.GetOperation<Customer>("test");

            _ = Task.Run(async () =>
            {
                await Task.Delay(100);
                _factory.SetResult<Customer>("test", new Customer { Id = Guid.NewGuid(), Name = "TEST" });
            });

            var customer = await op.GetResult();
            Assert.NotNull(customer);
            Assert.Equal("TEST", customer.Name);
            Assert.True(op.IsDisposed);
        }

        [Fact]
        public async Task WaitForResultShort()
        {
            _ = Task.Run(async () =>
            {
                await Task.Delay(100);
                _factory.SetResult<Customer>("test", new Customer { Id = Guid.NewGuid(), Name = "TEST" });
            });

            var customer = await _factory.GetResult<Customer>("test");
            Assert.NotNull(customer);
            Assert.Equal("TEST", customer.Name);         
        }

        [Fact]
        public async Task WaitForResultWithTimeout()
        {
            var op = _factory.GetOperation<Customer>("test");
            _ = Task.Run( async () =>
            {
                await Task.Delay(1000);
                _factory.SetResult<Customer>("test", new Customer { Id = Guid.NewGuid(), Name = "TEST" });
            });

            using (var cts = new CancellationTokenSource(100))
            {                
                var ex = await Assert.ThrowsAsync<TaskCanceledException>(() => op.GetResult(cts.Token));

                Assert.NotNull(ex);                
            }

            Assert.True(op.IsDisposed);
        }

        [Fact]
        public async Task WaitForResultWithTimeoutShort()
        {
            _ = Task.Run(async () =>
            {
                await Task.Delay(1000);
                _factory.SetResult<Customer>("test", new Customer { Id = Guid.NewGuid(), Name = "TEST" });
            });

            var ex = await Assert.ThrowsAsync<TaskCanceledException>(()=>
                _factory.GetResult<Customer>("test", TimeSpan.FromMilliseconds(100)));
            Assert.NotNull(ex);
        }     
    }
}
