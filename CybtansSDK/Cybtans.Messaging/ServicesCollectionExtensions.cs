using Cybtans.Messaging;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

#nullable enable

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServicesCollectionExtensions
    {
        public static IServiceCollection AddInternalMessageQueue(this IServiceCollection services, string exchange = "",
            Action<IMessageSubscriptionManager>? config = null)
        {
            services.TryAddSingleton<MessageSubscriptionManager>(provider =>
           {
               var subscriptionManager = new MessageSubscriptionManager(provider, exchange, provider.GetService<ILogger<MessageSubscriptionManager>>());
               config?.Invoke(subscriptionManager);
               return subscriptionManager;
           });

            services.AddSingleton<IMessageQueue>(provider =>
            {
                var subscriptionManager = provider.GetRequiredService<MessageSubscriptionManager>();
                return new InternalMessageQueue(subscriptionManager);
            });

            return services;
        }

    }
}
