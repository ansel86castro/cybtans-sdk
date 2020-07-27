using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;

namespace Cybtans.Testing
{
    public class TraceLoggerProvider : ILoggerProvider
    {
        public ILogger CreateLogger(string categoryName)
        {
            return new TraceLogger(categoryName);
        }

        public void Dispose()
        {
        }
    }

    public class TraceLogger : ILogger
    {
        private readonly string _categoryName;

        public TraceLogger(string categoryName)
        {
            _categoryName = categoryName;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return NoopDisposable.Instance;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            var msg = exception != null ?
                 $"{_categoryName}:[{logLevel}] {exception.Message} [{eventId}] {formatter(state, exception)} StackTace[{exception.StackTrace}]"
                : $"{_categoryName}:[{logLevel}] [{eventId}] {formatter(state, exception)}";

            switch (logLevel)
            {
                case LogLevel.Information:
                    Trace.TraceInformation(msg);
                    break;

                case LogLevel.Error:
                    Trace.TraceError(msg);
                    break;

                case LogLevel.Warning:
                    Trace.TraceWarning(msg);
                    break;

                case LogLevel.Critical:
                    Trace.Fail(msg);
                    break;

                default:
                    Trace.WriteLine(msg);
                    break;
            }
        }
    }
}