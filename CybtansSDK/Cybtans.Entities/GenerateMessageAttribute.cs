using System;
using System.Collections.Generic;

namespace Cybtans.Entities
{
    [AttributeUsage(AttributeTargets.Class| AttributeTargets.Enum)]
    public class GenerateMessageAttribute : Attribute
    {
        public GenerateMessageAttribute(string name = null)
        {
            this.Name = name;
        }

        public string Name { get; }

        public ServiceType Service { get; set; } =  ServiceType.None;

        public SecurityType Security { get; set; } = SecurityType.None;

        public string AllowedRead { get; set; }

        public string AllowedWrite { get; set; }
    }

    public enum SecurityType
    {
        None,
        Role,
        Policy,
        Authorized,
        AllowAnonymous
    }

    public enum ServiceType
    {
        /// <summary>
        /// Service interface with full class are generated
        /// </summary>
        Default,        
        /// <summary>
        /// Service interface only is generated
        /// </summary>
        Interface,   
        /// <summary>
        /// Service interface with partiaL class are generated
        /// </summary>
        Partial,
        /// <summary>
        /// Service is not generated
        /// </summary>
        None,
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class MessageExcludedAttribute : Attribute
    {

    }

}
