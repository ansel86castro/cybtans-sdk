#nullable enable

using Cybtans.Serialization;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace Cybtans.Messaging
{
    public class SubscriptionInfo<T> : BindingInfo
    {       
        private Type _type;
        private IMessageHandler<T>? _handler;

        public SubscriptionInfo(Type type, string exchange, string topic) : base(exchange, topic) 
        {           
            _type = type;
        }

        public SubscriptionInfo(Type type, BindingInfo info) 
            : this(type, info.Exchange, info.Topic)
        {

        }

        public SubscriptionInfo(IMessageHandler<T> handler, BindingInfo info)
         : base(info.Exchange, info.Topic)
        {
           
            _handler = handler;
            _type = _handler.GetType();
        }

        public SubscriptionInfo(IMessageHandler<T> handler, string exchange, string topic)
       : base(exchange, topic)
        {
          
            _handler = handler;
            _type = _handler.GetType();
        }


        public IMessageHandler<T>? Handler
        {
            get => _handler;
            set
            {
                _handler = value;
                if (value != null)
                {
                    _type = value.GetType();
                }
            }
        }

        public Type Type
        {
            get => _type; 
            set
            {
                _type = value;
                _handler = null;
            }
        }

        //private IMessageHandler<T> GetHandler(IServiceProvider? provider)
        //{
        //    if (_handler == null)
        //    {
        //        _handler = (IMessageHandler<T>)(provider?.GetService(_type) ?? Activator.CreateInstance(_type));
        //    }
        //    return _handler;
        //}

        internal override Task HandleMessage(IMessageSerializer serializer, IServiceProvider? provider, ReadOnlyMemory<byte> content)
        {
            IMessageHandler<T>? handler = _handler;
            if (provider != null)
            {
                handler = (IMessageHandler<T>)ActivatorUtilities.GetServiceOrCreateInstance(provider, _type);
            }
            else if (_handler == null)
            {
                handler = _handler = (IMessageHandler<T>)Activator.CreateInstance(_type);
            }

            if (handler == null)            
                return Task.CompletedTask;           

            var message = serializer.Deserialize<T>(content);
            return handler.HandleMessage(message);            
        }
    }
}
