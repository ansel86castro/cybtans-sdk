using Cybtans.Services.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
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
                        services.AddTransient(contract, info.Type);
                        break;
                    case LifeType.Scope:
                        services.AddScoped(contract, info.Type);
                        break;
                    case LifeType.Singleton:
                        services.AddSingleton(contract, info.Type);
                        break;
                }
            }

            return services;
        }

        public static IServiceCollection AddCybtansServices(this IServiceCollection services, string assemblyName)
        {            
            return AddCybtansServices(services, Assembly.LoadFrom(assemblyName));           
        }
    }
}
