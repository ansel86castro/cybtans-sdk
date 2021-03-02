using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cybtans.Graphics.Common
{
    /// <summary>
    /// Represents an object that delay his state changes until a call to CommitChanges is make.
    /// </summary>
    public interface IDeferreable
    {
        /// <summary>
        /// Perform the operations pendings and update the state of the object
        /// </summary>
        void CommitChanges();
    }

    public static class Deferreable
    {
        public static T Commit<T>(T obj) where T : IDeferreable
        {
            obj.CommitChanges();
            return obj;
        }
    }
}
