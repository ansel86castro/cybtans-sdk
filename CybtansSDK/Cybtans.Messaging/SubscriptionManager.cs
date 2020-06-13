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

        public SubscriptionManager(IServiceProvider? serviceProvider = null)
        {
            _serviceProvider = serviceProvider;
        }

        public BindingInfo? GetBindingForType(Type type)
        {
            BindingInfo info;

            bool lockTaken = false;
            try
            {
                _spinLock.Enter(ref lockTaken);

                if (!_exchages.TryGetValue(type, out info))
                {
                    var attr = type.GetCustomAttribute<ExchangeRouteAttribute>();
                    if (attr == null)
                        return null;

                    var exchange = attr.Exchange;                    
                    info = new BindingInfo(attr.Exchange, attr.Topic ?? type.FullName);
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

        public bool GetExchangeValues(Type type, out string? exchange, out string? topic)
        {
            exchange = null;
            topic = null;

            bool lockTaken = false;
            try
            {                
                _spinLock.Enter(ref lockTaken);
                
                if (_exchages.TryGetValue(type, out var info))
                {
                    exchange = info.Exchange;
                    topic = info.Topic;
                    return true;
                }

                var attr = type.GetCustomAttribute<ExchangeRouteAttribute>();
                if (attr == null)
                    return false;

                exchange = attr.Exchange;
                topic = attr.Topic ?? type.FullName;

                _exchages.Add(type, new BindingInfo(exchange, topic));
            }
            finally
            {
                if (lockTaken)
                {
                    _spinLock.Exit(true);
                }
            }

            return true;
        }

        public SubscriptionInfo<TMessage> Subscribe<TMessage, THandler>(string? exchange = null, string? topic = null)
          where THandler : IMessageHandler<TMessage>
        {
            bool lockTaken = false;
            try
            {
                _spinLock.Enter(ref lockTaken);

                var type = typeof(TMessage);
                SubscriptionInfo<TMessage> handlerInfo;

                if (!_exchages.TryGetValue(type, out var info))
                {
                    if(exchange == null)
                    {
                        var attr = type.GetCustomAttribute<ExchangeRouteAttribute>();
                        if (attr == null)
                            throw new QueueSubscribeException($"Exchange information not found for {type}");

                        exchange = attr.Exchange;
                        topic = attr.Topic ?? type.FullName;
                    }

                    handlerInfo = new SubscriptionInfo<TMessage>(_serviceProvider, typeof(THandler), exchange, topic ?? type.FullName);
                    _exchages.Add(type, handlerInfo);
                }
                else if(info.GetType() == typeof(BindingInfo))
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
