using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cybtans.Services
{
    public enum LifeType
    {
        /// <summary>
        /// defult life type. Objects are not cached so the object is always created when requesting the dependence.
        /// If the object is disposable you must ensure to call Dispose
        /// </summary>
        Transient,
        /// <summary>
        /// The object is singleton. The same instance is returned allways 
        /// </summary>
        Singleton,
        /// <summary>
        /// The object is cached per scope
        /// </summary>
        Scope
    }
}
