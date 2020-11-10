using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Cybtans.Services.Locking
{
    public interface ILockProvider
    {
       ValueTask<ILock> GetLock(string key);
    }
}
