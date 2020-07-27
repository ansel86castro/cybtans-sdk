using Cybtans.Tests.Core.Xunit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace Cybtans.Testing
{
    public interface IConfigurationProvider
    {
        IConfigurationRoot Configuration { get; }
    }

    public class BaseFixture : IDisposable, IAsyncLifetime, IConfigurationProvider
    {             
        public BaseFixture()
        {
            Configuration = CreateConfiguration();
        }

        public IConfigurationRoot Configuration { get; }
       
        public virtual Task InitializeAsync()
        {
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
        }

        public virtual Task DisposeAsync()
        {
            return Task.CompletedTask;
        }

        public static IConfigurationRoot CreateConfiguration()
        {
            var configBuilder = new ConfigurationBuilder()
              .SetBasePath(Directory.GetCurrentDirectory())
              .AddJsonFile("appsettings.json", optional: true)
              .AddEnvironmentVariables("CYBTANS_TEST_");

            return configBuilder.Build();
        }
    }
}