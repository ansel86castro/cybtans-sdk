using System;

namespace Cybtans.Testing
{
    internal class NoopDisposable : IDisposable
    {
        public static NoopDisposable Instance = new NoopDisposable();

        public void Dispose()
        { }
    }
}