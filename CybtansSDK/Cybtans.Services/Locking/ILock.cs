using System;
using System.Threading.Tasks;

namespace Cybtans.Services.Locking
{
    public interface ILock: IDisposable
    {
        Task RenewLock(TimeSpan time);
    }
}
