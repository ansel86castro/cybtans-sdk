using Cybtans.Messaging;
using Cybtans.Messaging.AzureServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

#nullable enable

namespace Microsoft.Extensions.DependencyInjection
{
    public interface IMessageQueueBuilder
    {
        IMessageQueueBuilder ConfigureOptions(Action<ServiceBusOptions> action);

        IMessageQueueBuilder ConfigureSubscriptions(Action<IMessageSubscriptionManager> action);
    }


    internal class MessageQueueBuilder : IMessageQueueBuilder
    {
        private Action<ServiceBusOptions>? _configureOptions;
        private Action<IMessageSubscriptionManager>? _configureSubscription;        
        private IConfiguration? _configuration;
        private IMessageSerializer _messageSerializer;

        public MessageQueueBuilder(IConfiguration? configuration)
        {
            _configuration = configuration;
            _messageSerializer = new CybtansMessageSerializer();
        }

        public void UseJsonSerializer()
        {
            _messageSerializer = new JsonMessageSerializer();
        }

        public void UseSerializer(IMessageSerializer serializer)
        {
            _messageSerializer = serializer;
        }

        public IMessageQueueBuilder ConfigureOptions(Action<ServiceBusOptions> action)
        {
            this._configureOptions = action;
            return this;
        }

        public IMessageQueueBuilder ConfigureSubscriptions(Action<IMessageSubscriptionManager> action)
        {
            this._configureSubscription = action;
            return this;
        }      

        internal void AddMessageQueue(IServiceCollection services)
        {
            AddSubscriptionManager(services);
            services.TryAddSingleton<IMessageQueue>(provider =>
            {
                var options = provider.GetRequiredService<ServiceBusOptions>();
                var subscriptionManager = provider.GetRequiredService<MessageSubscriptionManager>();             
                return new ServiceBusMessageQueue(options, subscriptionManager, provider.GetService<ILogger<ServiceBusMessageQueue>>(), _messageSerializer);
            });
        }

        internal void AddSubscriptionManager(IServiceCollection services)
        {
            services.TryAddSingleton(provider => CreateOptions());
            services.TryAddSingleton(provider =>
            {
                var options = provider.GetRequiredService<ServiceBusOptions>();
                var subscriptionManager = new MessageSubscriptionManager(provider, options.Topic.Name, provider.GetService<ILogger<MessageSubscriptionManager>>());
                _configureSubscription?.Invoke(subscriptionManager);

                return subscriptionManager;
            });
        }

        private ServiceBusOptions CreateOptions()
        {
            var options = new ServiceBusOptions();
            if (_configuration != null)
            {
                ConfigurationBinder.Bind(_configuration, "ServiceBusOptions", options);
            }
            _configureOptions?.Invoke(options);
            return options;
        }

    }

    public static class MessageQueueServiceCollectionExtensions
    {
        public static IMessageQueueBuilder AddAzureServiceBusMessageQueue(this IServiceCollection services, IConfiguration? configuration = null)
        {
            MessageQueueBuilder builder = new MessageQueueBuilder(configuration);
            builder.AddMessageQueue(services);
            return builder;
        }      

        public static async Task StartMessageQueueAsync(this IServiceProvider provider)
        {
            var queue = (ServiceBusMessageQueue)provider.GetRequiredService<IMessageQueue>();
            await queue.StartAsync();
        }

    }   
}
