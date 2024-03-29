﻿using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading.Tasks;

#nullable enable

namespace Cybtans.Messaging
{
    public interface IMessageSubscriptionManager
    {   
        void Subscribe<TMessage, THandler>(string? exchange = null, string? topic = null) where THandler : IMessageHandler<TMessage>;
        void Subscribe<TMessage>(IMessageHandler<TMessage> handler, string? exchange = null, string? topic = null);
        void Unsubscribe<TMessage, THandler>(string? exchange = null, string? topic = null) where THandler : IMessageHandler<TMessage>;
        void RegisterBinding<T>(string exchage, string? topic = null);
    }

    public interface IMessageQueue :IMessageSubscriptionManager, IDisposable
    {
        Task Publish(byte[] bytes, string? exchange, string topic);

        Task Publish(object message, string? exchange = null, string? topic = null);
       
        BindingInfo? GetBinding(Type type, string topic);
    
        void Start();        

    }
}
