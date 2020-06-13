#nullable enable

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Cybtans.Messaging
{    

    public class SubscriptionManager
    {      
        readonly Dictionary<Type, BindingInfo> _exchages = new Dictionary<Type, BindingInfo>();
        SpinLock _spinLock = new SpinLock();
        readonly IServiceProvider? _serviceProvider;
        readonly Dictionary<string, BindingInfo> _exchageBidings = new Dictionary<string, BindingInfo>();
        string? _globalExchange;

        public SubscriptionManager(string? globalExchange= null, IServiceProvider? serviceProvider = null)
        {
            _serviceProvider = serviceProvider;
            _globalExchange = globalExchange;
        }

        public BindingInfo? GetBindingForType(Type type) => GetBindingForType(type, (exchage, topic) => new BindingInfo(exchage, topic));
        private BindingInfo? GetBindingForType(Type type, Func<string, string, BindingInfo> createBindingFunc)
        {
            BindingInfo info;

            bool lockTaken = false;
            try
            {
                _spinLock.Enter(ref lockTaken);
                string? exchange;
                string topic;

                if (!_exchages.TryGetValue(type, out info))
                {
                    var attr = type.GetCustomAttribute<ExchangeRouteAttribute>();                    
                    exchange = attr?.Exchange ?? _globalExchange;
                    if(exchange == null)
                    {
                        return null;
                    }

                    topic = attr?.Topic ?? type.FullName;
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

        public bool GetExchangeValues(object message, out string? exchange, out string? topic)
        {
            Type type = message.GetType();
            var binding = GetBindingForType(type);
            exchange = binding?.Exchange;
            topic = binding?.Topic;
                                   
            return binding != null;
        }

        public void RegisterBinding<T>(string exchange, string? topic = null)
        {            
            Type type = typeof(T);

            bool lockTaken = false;
            try
            {
                _spinLock.Enter(ref lockTaken);
                if (!_exchages.TryGetValue(type, out var info))
                {
                    info = new BindingInfo(exchange, topic ?? type.FullName);
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

            var info = GetBindingForType(type, (exchange, topic) => new SubscriptionInfo<TMessage>(_serviceProvider, typeof(THandler), exchange, topic));
            if (info == null)
            {
                if (exchange != null)
                {
                    info = new BindingInfo(exchange, topic ?? type.FullName);
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
                    handlerInfo = new SubscriptionInfo<TMessage>(_serviceProvider, typeof(THandler), info);
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

            var info = GetBindingForType(type, (exchange, topic) => new SubscriptionInfo<TMessage>(_serviceProvider, handler, exchange, topic));
            if (info == null)
            {
                if (exchange != null)
                {
                    info = new BindingInfo(exchange, topic ?? type.FullName);
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
                    handlerInfo = new SubscriptionInfo<TMessage>(_serviceProvider, handler, info);
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

                topic ??= type.FullName;
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

        public Task HandleMessage(string exchange, string topic, byte[] data)
        {
            var key = BindingInfo.GetKey(exchange, topic);

            if (_exchageBidings.TryGetValue(key, out var info))
            {
                return info.HandleMessage(data);
            }

            return Task.CompletedTask;
        }
   

    }
}
