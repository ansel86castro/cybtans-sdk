using Cybtans.Entities;
using System;
using System.ComponentModel;

namespace Cybtans.Test.Domain
{
    [Description("Enum Type Description")]
    [GenerateMessage]
    public enum OrderTypeEnum
    {       
        [Description("Default")]
        Default,
        
        [Description("Normal")]
        Normal,

        [Description("Shipping")]
        [Obsolete]
        Shipping
    }
}
