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

        public bool Service { get; set; } = false;

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

    [AttributeUsage(AttributeTargets.Property)]
    public class MessageExcludedAttribute : Attribute
    {

    }

}
