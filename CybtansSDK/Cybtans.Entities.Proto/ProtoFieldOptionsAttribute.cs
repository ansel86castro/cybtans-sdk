using System;

namespace Cybtans.Entities
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class ProtoFieldAttribute : Attribute
    {
        public object Default { get; set; }

        public bool TsPartial { get; set; }

        public bool TsOptional { get; set; }
    }
}
