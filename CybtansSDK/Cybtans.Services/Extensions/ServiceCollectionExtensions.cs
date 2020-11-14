using Cybtans.Services.DependencyInjection;
using Cybtans.Services.Locking;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Cybtans.Services.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCybtansServices(this IServiceCollection services, params Assembly[] assemblies)
        {
            AssemblyTypeResolver resolver = new AssemblyTypeResolver(assemblies);
            resolver.Init();

            foreach (var kv in resolver.Dependencies)
            {
                var contract = kv.Key;
                var info = kv.Value;

                switch (info.LifeType)
                {
                    case LifeType.Transient:
                        services.TryAddTransient(contract, info.Type);
                        break;
                    case LifeType.Scope:
                        services.TryAddScoped(contract, info.Type);
                        break;
                    case LifeType.Singleton:
                        services.TryAddSingleton(contract, info.Type);
                        break;
                }
            }

            return services;
        }

        public static IServiceCollection AddCybtansServices(this IServiceCollection services, string assemblyName)
        {            
            return AddCybtansServices(services, Assembly.LoadFrom(assemblyName));           
        }

        public static IServiceCollection AddMemoryLockProvider(this IServiceCollection services)
        {
            services.TryAddSingleton<ILockProvider, MemoryLockProvider>();
            return services;
        }
    }
}
