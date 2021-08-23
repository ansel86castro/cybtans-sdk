using Cybtans.Entities;
using Cybtans.Entities.MongoDb;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace Microsoft.Extensions.DependencyInjection
{

    public static  class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMongoDbProvider(this IServiceCollection services, Action<MongoOptions> configure)=>AddMongoDbProvider<MongoClientProvider>(services, configure);

        public static IServiceCollection AddMongoDbProvider<T>(this IServiceCollection services, Action<MongoOptions> configure)
            where T: class, IMongoClientProvider
        {
            services.AddOptions();
            services.TryAddSingleton(srv =>
            {
                MongoOptions options = new MongoOptions();
                configure(options);
                if (options.ConnectionString == null)
                {
                    options.ConnectionString = Environment.GetEnvironmentVariable("MongoOptions__ConnectionString");
                    options.Database = Environment.GetEnvironmentVariable("MongoOptions__Database");
                }

                if (string.IsNullOrEmpty(options.ConnectionString) || string.IsNullOrEmpty(options.Database))
                {
                    throw new InvalidOperationException("MongoDb ConnectionString or Database not found");
                }

                return options;
            });

            services.AddSingleton<IMongoClientProvider, T>();
            return services;
        }

        public static IServiceCollection AddObjectRepositories(this IServiceCollection services)
        {
            services.TryAddSingleton(typeof(IObjectRepository<,>), typeof(MongoDbObjectRepository<,>));
            services.TryAddSingleton(typeof(IObjectRepository<>), typeof(MongoDbObjectRepository<>));
            return services;
        }
    }
}
