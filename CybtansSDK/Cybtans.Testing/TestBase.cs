using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using Xunit;
using Xunit.Abstractions;

namespace Cybtans.Testing
{
    public class TestBase<TFixture> : IClassFixture<TFixture>
        where TFixture : BaseFixture
    {
        public TestBase(TFixture fixture, ITestOutputHelper testOutputHelper)
        {
            Fixture = fixture;

            LoggerFactory = new LoggerFactory();
            if (testOutputHelper != null)
            {
                LoggerFactory.AddProvider(new XunitLoggerProvider(testOutputHelper));
            }

            LoggerFactory.AddProvider(new TraceLoggerProvider());
        }

        public TFixture Fixture { get; }
        public ILoggerFactory LoggerFactory { get; }

        public ILogger<T> LoggerFor<T>()
        {
            return LoggerFactory?.CreateLogger<T>();
        }

        public IOptions<T> OptionsOf<T>(string key = null)
            where T : class, new()
        {
            return Fixture.Configuration.GetOptions<T>(key);
        }      
    }

    public class TestBase : IDisposable
    {
        public TestBase(ITestOutputHelper testOutputHelper)
        {
            LoggerFactory = new LoggerFactory();
            if (testOutputHelper != null)
            {
                LoggerFactory.AddProvider(new XunitLoggerProvider(testOutputHelper));
            }
        }

        protected ILoggerFactory LoggerFactory { get; }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
        }
    }
}