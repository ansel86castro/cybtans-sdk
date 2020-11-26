using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Cybtans.Automation
{
    public static class WaitHandleExtensions
    {
        static Dictionary<WaitHandle, TaskCompletionSource<object>> _taskCache = new Dictionary<WaitHandle, TaskCompletionSource<object>>();

        public static async Task ToTask(this WaitHandle waitHandle, int timeoutMilis = -1)
        {
            if (_taskCache.TryGetValue(waitHandle, out var tcs))
            {
                await tcs.Task;
                return;
            }

            lock (_taskCache)
            {
                tcs = new TaskCompletionSource<object>();
                _taskCache.TryAdd(waitHandle, tcs);
            }

            // Registering callback to wait till WaitHandle changes its state

            var rwh = ThreadPool.RegisterWaitForSingleObject(
                    waitHandle,
                    (handler, timeout) =>
                    {
                        if (timeout)
                        {
                            tcs.TrySetCanceled();
                        }
                        else
                        {
                            tcs.TrySetResult(null);
                        }

                        lock (_taskCache)
                        {
                            _taskCache.Remove(waitHandle);
                        }
                    },
                    null,
                    timeoutMilis,
                    true);

            await tcs.Task;
            rwh.Unregister(null);
        }

        public static Task<T> ToTask<T>(this WaitHandle waitHandle, Action<TaskCompletionSource<T>> action, int timeoutMilis = -1)
        {
            var tcs = new TaskCompletionSource<T>();

            // Registering callback to wait till WaitHandle changes its state

            RegisteredWaitHandle rwh = ThreadPool.RegisterWaitForSingleObject(
                waitHandle,
                (o, timeout) =>
                {
                    if (timeout)
                    {
                        tcs.TrySetCanceled();
                    }
                    else
                    {
                        action(tcs);
                    }
                },
                null,
                timeoutMilis,
                true);

            var task = tcs.Task;
            task.ContinueWith(t =>
                rwh.Unregister(null));

            return task;
        }
    }
}
