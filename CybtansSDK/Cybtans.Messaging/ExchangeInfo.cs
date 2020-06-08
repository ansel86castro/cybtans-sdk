#nullable enable

using System.Threading.Tasks;

namespace Cybtans.Messaging
{
    public class BindingInfo
    {
        public string Exchange { get; }

        public string Topic { get; }

        public string Key => $"{Exchange}:{Topic}";

        public static string GetKey(string exchange, string topic) => $"{exchange}:{topic}";

        public BindingInfo(string exchange, string topic)
        {
            Exchange = exchange;
            Topic = topic;
        }

        public virtual Task HandleMessage(byte[] message)
        {
            return Task.CompletedTask;
        }
    }
}
