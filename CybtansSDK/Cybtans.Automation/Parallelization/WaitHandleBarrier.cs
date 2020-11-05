using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

#nullable enable

namespace Cybtans.Automation
{
    public class WaitHandleBarrier : IDisposable
    {
        RegisteredWaitHandle? _registeredWaitHandle;      
        ManualResetEvent? _handle;
        List<TaskCompletionSource<object?>> _taskCompletionSources = new List<TaskCompletionSource<object?>>();
        bool _disposed;
        bool _completed;
        int _referenceCount;

        public WaitHandleBarrier(string name)
        {
            Name = name;
            _handle = new ManualResetEvent(false);
            _registeredWaitHandle = ThreadPool.RegisterWaitForSingleObject(_handle, OnRun, null, BarrierManager.WaitHandleTimeout, true);
        }

        public string Name { get; }


        public EventWaitHandle? Handle => _handle;

        public bool Set()
        {
            if (_handle == null)
                throw new InvalidOperationException("Handler already disposed");

            return _handle.Set();
        }

        public void Dispose()
        {
            if (_disposed) return;

            _disposed = true;            
            _registeredWaitHandle?.Unregister(null);
            _handle?.Dispose();

            _registeredWaitHandle = null;
            _handle = null;
        }

        public Task CreateTask()
        {
            if (_completed)
                return Task.CompletedTask;

            var tcs = new TaskCompletionSource<object?>(TaskCreationOptions.RunContinuationsAsynchronously);
            lock (_taskCompletionSources)
            {
                _taskCompletionSources.Add(tcs);
            }

            return tcs.Task;
        }

        private void OnRun(object? state, bool timedOut)
        {
            lock (_taskCompletionSources)
            {
                var tasks = _taskCompletionSources.ToArray();
                _taskCompletionSources.Clear();

                foreach (var tcs in tasks)
                {
                    if (timedOut)
                    {
                        tcs.SetCanceled();
                    }
                    else
                    {
                        tcs.SetResult(null);
                    }
                }

                _completed = true;
            }

            _registeredWaitHandle?.Unregister(null);
            _handle?.Dispose();
        }

        public override string ToString()
        {
            return Name;
        }

        public int AddReference()
        {
            return Interlocked.Increment(ref _referenceCount);
        }

        public int RemoveReference()
        {
           return Interlocked.Decrement(ref _referenceCount);
        }
    }

}

#nullable restore
