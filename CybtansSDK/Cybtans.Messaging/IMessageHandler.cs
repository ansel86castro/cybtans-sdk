using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Cybtans.Messaging
{
    public interface IMessageHandler<T>        
    {
        Task HandleMessage(T message);
    }
}
