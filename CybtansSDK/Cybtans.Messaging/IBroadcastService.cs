using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Cybtans.Messaging
{

    public interface IBroadcastService: IDisposable
    {
        IBroadcastSubscriptionManager Subscriptions { get; }

        Task Publish(byte[] bytes, string channel);

        Task Publish(object message, string? channel = null);

        void Start();
    }
}
