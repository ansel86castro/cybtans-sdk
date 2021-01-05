#nullable enable

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Cybtans.Messaging
{

    public class MessageSubscriptionManager : IMessageSubscriptionManager
    {      
        readonly Dictionary<Type, BindingInfo> _exchages = new Dictionary<Type, BindingInfo>();
        SpinLock _spinLock = new SpinLock();        
        readonly Dictionary<string, BindingInfo> _exchageBidings = new Dictionary<string, BindingInfo>();
        readonly string? _globalExchange;
        readonly IServiceProvider? _serviceProvider;
        private readonly ILogger<MessageSubscriptionManager>? _logger;

        public MessageSubscriptionManager(IServiceProvider? provider = null, string? globalExchange= null, ILogger<MessageSubscriptionManager> logger = null)
        {            
            _globalExchange = globalExchange;
            _serviceProvider = provider;
            _logger = logger;
        }

        public IEnumerable<BindingInfo> GetBindings() => _exchages.Values;

        public BindingInfo? GetBindingForType(Type type, string? exchange = null, string? topic = null) => GetBindingForType(type, exchange, topic, (exchage, topic) => new BindingInfo(exchage, topic));
       
        private BindingInfo? GetBindingForType(Type type, string? exchange, string? topic, Func<string, string, BindingInfo> createBindingFunc)
        {
            BindingInfo info;

            bool lockTaken = false;
            try
            {
                _spinLock.Enter(ref lockTaken);                                

                if (!_exchages.TryGetValue(type, out info))
                {
                    var attr = type.GetCustomAttribute<ExchangeRouteAttribute>();

                    if (exchange == null)
                    {
                        exchange = attr?.Exchange ?? _globalExchange;
                        if (exchange == null)
                            return null;
                    }

                    if (topic == null)
                    {
                        topic = attr?.Topic ?? type.Name;
                    }
                    info = createBindingFunc(exchange, topic);
                    _exchages.Add(type, info);
                }
                return info;
            }
            finally
            {
                if (lockTaken)
                {
                    _spinLock.Exit(true);
                }
            }
        }      

        public void RegisterBinding<TMessage>(string exchange, string? topic = null)
        {            
            Type type = typeof(TMessage);

            bool lockTaken = false;
            try
            {
                _spinLock.Enter(ref lockTaken);
                if (!_exchages.TryGetValue(type, out var info))
                {
                    info = new BindingInfo(exchange, topic ?? type.Name);
                    _exchages.Add(type, info);
                }
            }
            finally
            {
                if (lockTaken)
                {
                    _spinLock.Exit(true);
                }
            }
        }

        public SubscriptionInfo<TMessage> Subscribe<TMessage, THandler>(string? exchange = null, string? topic = null)
          where THandler : IMessageHandler<TMessage>
        {
            var type = typeof(TMessage);
            SubscriptionInfo<TMessage> handlerInfo;

            var info = GetBindingForType(type, exchange, topic, (exchange, topic) => new SubscriptionInfo<TMessage>(typeof(THandler), exchange, topic));
            if (info == null)
            {
                if (exchange != null)
                {
                    info = new BindingInfo(exchange, topic ?? type.Name);
                }
                else
                {
                    throw new InvalidOperationException($"Binding for {type} not found");
                }
            }

            bool lockTaken = false;            
            try
            {
                _spinLock.Enter(ref lockTaken);                

                if(info.GetType() == typeof(BindingInfo))
                {
                    handlerInfo = new SubscriptionInfo<TMessage>(typeof(THandler), info);
                    _exchages[type] = handlerInfo;
                }
                else
                {
                    handlerInfo = (SubscriptionInfo<TMessage>)info;
                    handlerInfo.Type = typeof(THandler);
                }

                _exchageBidings[handlerInfo.Key] = handlerInfo;

                return handlerInfo;
            }
            finally
            {
                if (lockTaken)
                {
                    _spinLock.Exit();
                }
            }
        }

        public SubscriptionInfo<TMessage> Subscribe<TMessage>(IMessageHandler<TMessage> handler, string? exchange = null, string? topic = null)        
        {
            var type = typeof(TMessage);
            SubscriptionInfo<TMessage> handlerInfo;

            var info = GetBindingForType(type, exchange, topic, (exchange, topic) => new SubscriptionInfo<TMessage>(handler, exchange, topic));
            if (info == null)
            {
                if (exchange != null)
                {
                    info = new BindingInfo(exchange, topic ?? type.Name);
                }
                else
                {
                    throw new InvalidOperationException($"Binding for {type} not found");
                }
            }

            bool lockTaken = false;
            try
            {
                _spinLock.Enter(ref lockTaken);

                if (info.GetType() == typeof(BindingInfo))
                {
                    handlerInfo = new SubscriptionInfo<TMessage>(handler, info);
                    _exchages[type] = handlerInfo;
                }
                else
                {
                    handlerInfo = (SubscriptionInfo<TMessage>)info;                
                    handlerInfo.Handler = handler;
                }

                _exchageBidings[handlerInfo.Key] = handlerInfo;

                return handlerInfo;
            }
            finally
            {
                if (lockTaken)
                {
                    _spinLock.Exit();
                }
            }
        }

        public void Unsubscribe<TMessage, THandler>(string? exchange = null, string? topic = null)
             where THandler : IMessageHandler<TMessage>
        {
            bool lockTaken = false;
            try
            {
                _spinLock.Enter(ref lockTaken);
                var type = typeof(TMessage);
                if (exchange == null)
                {                    
                    var attr =type.GetCustomAttribute<ExchangeRouteAttribute>();
                    if (attr == null)
                        throw new QueueSubscribeException($"Exchange information not found for {type}");

                    exchange = attr.Exchange;
                    topic = attr.Topic;
                }                

                topic ??= type.Name;
                var key = BindingInfo.GetKey(exchange, topic);
                _exchageBidings.Remove(key);
                _exchages.Remove(type);
            }
            finally
            {
                if (lockTaken)
                {
                    _spinLock.Exit();
                }
            }
        }

        public void Unsubscribe<TMessage>(string? exchange = null, string? topic = null)
        {
            bool lockTaken = false;
            try
            {
                _spinLock.Enter(ref lockTaken);
                var type = typeof(TMessage);
                if (exchange == null)
                {
                    var attr = type.GetCustomAttribute<ExchangeRouteAttribute>();
                    if (attr == null)
                        throw new QueueSubscribeException($"Exchange information not found for {type}");

                    exchange = attr.Exchange;
                    topic = attr.Topic;
                }

                topic ??= type.Name;
                var key = BindingInfo.GetKey(exchange, topic);
                _exchageBidings.Remove(key);
                _exchages.Remove(type);
            }
            finally
            {
                if (lockTaken)
                {
                    _spinLock.Exit();
                }
            }
        }
       

        public async Task HandleMessage(string exchange, string topic, byte[] data)
        {
            var key = BindingInfo.GetKey(exchange, topic);

            if (_exchageBidings.TryGetValue(key, out var info))
            {
                _logger?.LogDebug("Handler found with key {Key}", key);

                if (_serviceProvider != null)
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        _logger?.LogDebug("Executing Handler {Key} {Type}", key, info.GetType());
                        await info.HandleMessage(scope.ServiceProvider, data);
                    }
                }
                else
                {
                    _logger?.LogDebug("Executing Handler {Key}", key);
                    await info.HandleMessage(null, data);
                }
            }
            else
            {
                _logger?.LogDebug("Handler not found for {Key}", key);
            }
        }

        void IMessageSubscriptionManager.Subscribe<TMessage, THandler>(string? exchange, string? topic)
        {
            Subscribe<TMessage, THandler>(exchange, topic);
        }

        void IMessageSubscriptionManager.Subscribe<TMessage>(IMessageHandler<TMessage> handler, string? exchange, string? topic)
        {
            Subscribe<TMessage>(handler, exchange, topic);
        }

      
    }
}
