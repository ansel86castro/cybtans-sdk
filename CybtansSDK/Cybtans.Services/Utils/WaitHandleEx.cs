using System;
using System.Threading;
using System.Threading.Tasks;

namespace Cybtans.Services.Utils
{
    public static class WaitHandleEx
    {
        public static Task ToTask(this WaitHandle waitHandle, int timeoutMilis = -1)
        {
            var tcs = new TaskCompletionSource<object>();

            // Registering callback to wait till WaitHandle changes its state

            var rwh = ThreadPool.RegisterWaitForSingleObject(
                waitHandle,
                (o, timeout) => { tcs.TrySetResult(null); },
                 null,
                timeoutMilis,
                true);

            var task = tcs.Task;
            task.ContinueWith(t => rwh.Unregister(null));
            return task;
        }

        public static Task ToTask(this WaitHandle waitHandle, Action<TaskCompletionSource<object>> setAction)
        {
            var tcs = new TaskCompletionSource<object>();

            // Registering callback to wait till WaitHandle changes its state

            var rwh = ThreadPool.RegisterWaitForSingleObject(
                waitHandle,
                (o, timeout) => { setAction(tcs); },
                null,
                -1,
                true);

            var task = tcs.Task;
            task.ContinueWith(t => rwh.Unregister(null));

            return task;
        }
    }
}
