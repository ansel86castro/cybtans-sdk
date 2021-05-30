using Cybtans.Services.FileStorage;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAzureFileStorage(this IServiceCollection services, Action<AzureStorageConfig> configureAction)
        {
            services.Configure(configureAction);
            services.TryAddSingleton<IFileStorage, AzureFileStorage>();
            return services;
        }

    }
}
