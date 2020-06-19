using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cybtans.Services.DependencyInjection
{

    public class RegisterDependencyAttribute : Attribute
    {
        /// <summary>
        /// Specify the Interface the decorated type implements using Scoped Life Type
        /// </summary>
        /// <param name="contract">Interface</param>
        public RegisterDependencyAttribute(Type contract)
        {
            Contract = contract;
        }

        /// <summary>
        ///  Specify the Interfaces the decorated type implements using Scoped Life Type
        /// </summary>
        /// <param name="contracts">Interfaces</param>
        public RegisterDependencyAttribute(Type[] contracts)
        {
            Contracts = contracts;
        }

        public Type Contract { get; set; }

        /// <summary>
        /// The lifetime for the dependency. Has LifeType.Scope as default value
        /// </summary>
        public LifeType LifeType { get; set; } = LifeType.Scope;

        public Type[] Contracts { get; set; }
    }
}
