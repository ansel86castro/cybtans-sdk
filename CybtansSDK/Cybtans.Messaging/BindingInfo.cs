#nullable enable

using System;
using System.Threading.Tasks;

namespace Cybtans.Messaging
{
    public class BindingInfo
    {
        public string Exchange { get; }

        public string Topic { get; }

        public string Key => $"{Exchange}:{Topic}";
       
        public BindingInfo(string exchange, string topic)
        {
            Exchange = exchange;
            Topic = topic;
        }

        public static string GetKey(string exchange, string topic) => $"{exchange}:{topic}";

        internal virtual Task HandleMessage(IMessageSerializer serializer, IServiceProvider? provider, ReadOnlyMemory<byte> message)
        {
            return Task.CompletedTask;
        }
    }
}
