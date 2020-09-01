using System;

namespace Cybtans.Entities
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Enum)]
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

}
