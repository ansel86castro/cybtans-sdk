using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

#nullable enable

namespace Cybtans.Services
{
    public interface IAwaitableOperation: IDisposable
    {
        bool IsDisposed { get; }      

        void SetResult(object value);

        void Cancel();

        void SetException(Exception e);
    }

    public interface IAwaitableOperation<T> : IAwaitableOperation
    {
        Task<T> GetResult(CancellationToken ct = default);

        void SetResult(T value);
    }


    public class WaitOperationManager
    {
        ConcurrentDictionary<string, IAwaitableOperation> _operations = new ConcurrentDictionary<string, IAwaitableOperation>();

        public IAwaitableOperation<T> GetOperation<T>(string key)
        {
            return  (IAwaitableOperation<T>)_operations.GetOrAdd(key, key => new AwaitableOperation<T>(key, this));
        }

        public Task<T> GetResult<T>(string key)
        {
            var op = GetOperation<T>(key);
            return op.GetResult();
        }

        public async Task<T> GetResult<T>(string key, TimeSpan timeout)
        {
            var op = GetOperation<T>(key);
            using (var cts = new CancellationTokenSource(timeout))
            {
                return await op.GetResult(cts.Token);
            }
        }

        public void SetResult<T>(string key, T value)
        {
            var op = GetOperation<T>(key);
            op.SetResult(value);
        }

        private void Remove(string key)
        {
            _operations.TryRemove(key, out _);
        }

        public class AwaitableOperation<T> : IAwaitableOperation<T>
        {
            private string _key;
            private readonly WaitOperationManager _factory;
            private TaskCompletionSource<T> _tcs;
            private bool disposed;

            public AwaitableOperation(string key, WaitOperationManager factory)
            {
                _key = key;
                _factory = factory;
                _tcs = new TaskCompletionSource<T>();                
            }

            public string Key => _key;

            public bool IsDisposed => disposed;

            public void Dispose()
            {
                if (!disposed)
                {
                    _factory.Remove(Key);
                    disposed = true;
                }
            }

            public async Task<T> GetResult(CancellationToken ct = default)
            {
                CheckDisposed();

                if (ct.CanBeCanceled)
                {
                    ct.Register(() =>
                    {
                        _tcs.SetCanceled();

                    });
                }

                try
                {
                    return await _tcs.Task;
                }
                finally
                {
                    Dispose();
                }
            }

            public void SetResult(T value)
            {
                CheckDisposed();

                _tcs.SetResult(value);                
            }

            public void SetResult(object value)
            {
                CheckDisposed();

                _tcs.SetResult((T)value);
            }

            public async Task WaitForCompletion(CancellationToken ct = default)
            {
                CheckDisposed();

                if (ct.CanBeCanceled)
                {
                    ct.Register(() =>
                    {
                        _tcs.SetCanceled();

                    });
                }
                try
                {
                    await _tcs.Task;
                }
                finally
                {
                    Dispose();
                }
            }

            private void CheckDisposed()
            {
                if (disposed) throw new InvalidOperationException("The operation has been disposed");
            }

            public void Cancel()
            {
                CheckDisposed();

                _tcs.SetCanceled();
            }

            public void SetException(Exception e)
            {
                CheckDisposed();

                _tcs.SetException(e);
            }
        }
    }

}

  
  
