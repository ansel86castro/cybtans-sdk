using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Cybtans.Messaging
{
    public interface IBroacastService
    {
        void Subscribe<TMessage, THandler>()
          where THandler : IMessageHandler<TMessage>;

        void Unsubscribe<TMessage, THandler>()
            where THandler : IMessageHandler<TMessage>;

        Task Publish(object message ,string? key= null);
    }
}
