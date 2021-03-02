using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Cybtans.Graphics.Common
{
    /// <summary>
    /// Represents an object that may move or change his state within the time
    /// </summary>
    public interface IDynamic
    {
        /// <summary>
        /// Update  the state of the object based on the elapsed time
        /// </summary>
        /// <param name="DeltaT">elapsed time since the last update expresed in seconds</param>
        void Update(float elapsedTime);
        
    }
}
