#nullable enable

using Cybtans.Serialization;
using System;
using System.Threading.Tasks;

namespace Cybtans.Messaging
{
    public class SubscriptionInfo<T> : BindingInfo
    {
        private readonly IServiceProvider? _serviceProvider;
        private Type _type;
        private IMessageHandler<T>? _handler;

        public SubscriptionInfo(IServiceProvider? serviceProvider, Type type, string exchange, string topic) : base(exchange, topic) 
        {
            _serviceProvider = serviceProvider;
            _type = type;
        }

        public SubscriptionInfo(IServiceProvider? serviceProvider, Type type, BindingInfo info) 
            : this(serviceProvider, type, info.Exchange, info.Topic)
        {

        }

        public SubscriptionInfo(IServiceProvider? serviceProvider, IMessageHandler<T> handler, BindingInfo info)
         : base(info.Exchange, info.Topic)
        {
            _serviceProvider = serviceProvider;
            _handler = handler;
            _type = _handler.GetType();
        }

        public SubscriptionInfo(IServiceProvider? serviceProvider, IMessageHandler<T> handler, string exchange, string topic)
       : base(exchange, topic)
        {
            _serviceProvider = serviceProvider;
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

        private IMessageHandler<T> GetHandler()
        {
            if (_handler == null)
            {
                _handler = (IMessageHandler<T>)(_serviceProvider?.GetService(_type) ?? Activator.CreateInstance(_type));
            }
            return _handler;
        }

        public override Task HandleMessage(byte[] content)
        {
            var message = BinaryConvert.Deserialize<T>(content);            
            return GetHandler().HandleMessage(message);            
        }
    }
}
