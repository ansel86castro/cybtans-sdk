using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable

namespace Cybtans.Messaging
{
    public interface IMessageQueue
    {       
        void Subscribe<TMessage, THandler>(string? exchange = null, string? topic = null)          
            where THandler : IMessageHandler<TMessage>;

        void Unsubscribe<TMessage, THandler>(string? exchange = null, string? topic = null)
            where THandler : IMessageHandler<TMessage>;

        Task Publish(object message, string? exchange , string? topic);           

        public Task Publish(object message)
        {
            return Publish(message, null, null);
        }

        BindingInfo? GetBindingForType(Type type);

    }
}
