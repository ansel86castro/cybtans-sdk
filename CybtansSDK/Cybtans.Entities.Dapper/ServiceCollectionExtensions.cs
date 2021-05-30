using Cybtans.Entities;
using Cybtans.Entities.Dapper;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDatabaseConnectionFactory(this IServiceCollection services, Action<DatabaseConectionFactoryOptions> configure)
        {
            services.TryAddSingleton(sp =>
            {
                var options = new DatabaseConectionFactoryOptions();
                configure?.Invoke(options);
                return options;
            });

            services.TryAddSingleton<IDatabaseConnectionFactory, DatabaseConnectionFactory>();
            return services;
        }

        public static IServiceCollection AddDatabaseConnectionFactory(this IServiceCollection services, Action<IServiceProvider, DatabaseConectionFactoryOptions> configure)
        {
            services.TryAddSingleton(sp =>
            {
                var options = new DatabaseConectionFactoryOptions();
                configure?.Invoke(sp, options);
                return options;
            });

            services.TryAddSingleton<IDatabaseConnectionFactory, DatabaseConnectionFactory>();
            return services;
        }

    }
}
