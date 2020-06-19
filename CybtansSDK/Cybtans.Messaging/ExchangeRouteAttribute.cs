using System;
using System.Collections.Generic;
using System.Text;

#nullable enable

namespace Cybtans.Messaging
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ExchangeRouteAttribute:Attribute
    {
        public ExchangeRouteAttribute(string exchange, string? topic = null)
        {
            Exchange = exchange;
            Topic = topic;
        }

        public string Exchange { get; }

        public string? Topic { get; }
    }
}
