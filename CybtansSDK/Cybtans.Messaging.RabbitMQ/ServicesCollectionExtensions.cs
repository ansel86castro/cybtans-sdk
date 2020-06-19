using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System;
using System.Transactions;

#nullable enable

namespace Cybtans.Messaging.RabbitMQ
{
    public interface IMessageQueueBuilder
    {
        IMessageQueueBuilder ConfigureOptions(Action<RabbitMessageQueueOptions> action);

        IMessageQueueBuilder ConfigureSubscriptions(Action<IMessageSubscriptionManager> action);
    }

   
    internal class MessageQueueBuilder : IMessageQueueBuilder
    {
        private Action<RabbitMessageQueueOptions>? _configureOptions;
        private Action<IMessageSubscriptionManager>? _configureSubscription;
        private Action<ConnectionFactory>? _configureConnectionFactory;

        private IConfiguration _configuration;
        public MessageQueueBuilder(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IMessageQueueBuilder ConfigureOptions(Action<RabbitMessageQueueOptions> action)
        {
            this._configureOptions = action;
            return this;
        }

        public IMessageQueueBuilder ConfigureSubscriptions(Action<IMessageSubscriptionManager> action)
        {
            this._configureSubscription = action;
            return this;
        }

        public IMessageQueueBuilder ConfigureFactory(Action<ConnectionFactory> action)
        {
            _configureConnectionFactory = action;
            return this;
        }

        internal IMessageQueue Create(IServiceProvider provider)
        {
            var options = new RabbitMessageQueueOptions();
            ConfigurationBinder.Bind(_configuration, "RabbitMessageQueueOptions", options);
            _configureOptions?.Invoke(options);

            var factory = new ConnectionFactory() { HostName = options.Hostname };

            _configureConnectionFactory?.Invoke(factory);

            var subscriptionManager = new MessageSubscriptionManager(provider, options.Exchange.Name);
            var messageQueue = new RabbitMessageQueue(factory, subscriptionManager, options, provider.GetService<ILogger<RabbitMessageQueue>>());            
            _configureSubscription?.Invoke(subscriptionManager);            

            return messageQueue;
        }

        
    }

    public static class ServiceCollectionExtensions
    {
        public static IMessageQueueBuilder AddMessageQueue(this IServiceCollection services, IConfiguration configuration)
        {         
            MessageQueueBuilder builder = new MessageQueueBuilder(configuration);
            services.AddSingleton<IMessageQueue>(provider => builder.Create(provider));
            return builder;
        }

        public static void StartMessageQueue(this IServiceProvider provider)
        {
            var queue = provider.GetRequiredService<IMessageQueue>();
            queue.Start();
        }
       
    }
}
