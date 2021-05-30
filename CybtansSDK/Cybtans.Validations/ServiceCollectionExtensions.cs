using Cybtans.Validations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {        
        public static IServiceCollection AddDefaultValidatorProvider(this IServiceCollection services, Action<DefaultValidatorProvider> configure)
        {
            services.TryAddSingleton<IValidatorProvider>(sp =>
            {
                var service = new DefaultValidatorProvider();
                configure?.Invoke(service);
                return service;
            });

            return services;
        }       
    }
}
