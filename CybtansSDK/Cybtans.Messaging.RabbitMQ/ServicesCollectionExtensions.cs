using Cybtans.Messaging;
using Cybtans.Messaging.RabbitMQ;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System;

#nullable enable

namespace Microsoft.Extensions.DependencyInjection
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
      

        internal void AddMessageQueue(IServiceCollection services)
        {
            AddSubscriptionManager(services);
            services.TryAddSingleton<IMessageQueue>(provider =>
            {
                var options = provider.GetRequiredService<RabbitMessageQueueOptions>();
                var subscriptionManager = provider.GetRequiredService<MessageSubscriptionManager>();

                var factory = new ConnectionFactory() { HostName = options.Hostname };
                _configureConnectionFactory?.Invoke(factory);

                return new RabbitMessageQueue(factory, subscriptionManager, options, provider.GetService<ILogger<RabbitMessageQueue>>());
            });
        }

        internal void AddSubscriptionManager(IServiceCollection services)
        {
            services.TryAddSingleton(provider => CreateOptions());
            services.TryAddSingleton(provider =>
            {
                var options = provider.GetRequiredService<RabbitMessageQueueOptions>();
                var subscriptionManager =  new MessageSubscriptionManager(provider, options.Exchange.Name, provider.GetService<ILogger<MessageSubscriptionManager>>());
                _configureSubscription?.Invoke(subscriptionManager);

                return subscriptionManager;
            });            
        }

        private RabbitMessageQueueOptions CreateOptions()
        {
            var options = new RabbitMessageQueueOptions();
            ConfigurationBinder.Bind(_configuration, "RabbitMessageQueueOptions", options);
            _configureOptions?.Invoke(options);
            return options;
        }

    }

    public static class MessageQueueServiceCollectionExtensions
    {       
        public static IMessageQueueBuilder AddMessageQueue(this IServiceCollection services, IConfiguration configuration)
        {         
            MessageQueueBuilder builder = new MessageQueueBuilder(configuration);
            builder.AddMessageQueue(services);            
            return builder;
        }

        public static void StartMessageQueue(this IServiceProvider provider)
        {
            var queue = provider.GetRequiredService<IMessageQueue>();
            queue.Start();
        }
       
    }

    public static class BroadCastServiceCollectionExtensions
    {
        public static IServiceCollection AddBroadCastService(
            this IServiceCollection services, 
            BroadcastServiceOptions options,  
            Action<IBroadcastSubscriptionManager>? subcriptionConfig = null,
            Action<ConnectionFactory>? connectionConfig = null)
        {
            services.TryAddSingleton<IBroadcastService>(p =>
            {
                var loggerFactory = p.GetService<ILoggerFactory>();

                var manager = new BroadcastSubscriptionManager(options, p, loggerFactory);
                subcriptionConfig?.Invoke(manager);

                var factory = new ConnectionFactory() { HostName = options.Hostname };
                connectionConfig?.Invoke(factory);                
                
                return new RabbitBroadCastService(options, factory, manager, loggerFactory);
            });
            return services;
        }

        public static void StartBroadCastService(this IServiceProvider provider)
        {
            var queue = provider.GetRequiredService<IBroadcastService>();
            queue.Start();
        }
    }
}
