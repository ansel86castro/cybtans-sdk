using Cybtans.Refit;
using Cybtans.Serialization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Refit;
using System;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
		public static IHttpClientBuilder AddClient<T>(this IServiceCollection services, IConfiguration configuration)
            where T:class
		{
            var serviceName = typeof(T).Name;
            return AddClient<T>(services, configuration.GetSection($"{serviceName}:BaseUrl"));   
        }

        public static IHttpClientBuilder AddClient<T>(this IServiceCollection services, string baseUrl)
            where T : class
        {
            var serviceName = typeof(T).Name;

            var settings = new RefitSettings();
            settings.ContentSerializer = new CybtansContentSerializer(settings.ContentSerializer);

           return services.AddHttpClient(serviceName, c =>
            {
                c.BaseAddress = new Uri(baseUrl);
                c.DefaultRequestHeaders.Add("Accept", $"{BinarySerializer.MEDIA_TYPE}; charset={Encoding.UTF8.WebName}");
            })
            .AddTypedClient(c => RestService.For<T>(c, settings));            
        }

        public static IHttpClientBuilder AddClient(this IServiceCollection services, Type interfaceType, string baseUrl)
        {
            var serviceName = interfaceType.Name;

            var settings = new RefitSettings();
            settings.ContentSerializer = new CybtansContentSerializer(settings.ContentSerializer);

            var builder = services.AddHttpClient(serviceName, c =>
            {
                c.BaseAddress = new Uri(baseUrl);
                c.DefaultRequestHeaders.Add("Accept", $"{BinarySerializer.MEDIA_TYPE}; charset={Encoding.UTF8.WebName}");
            });

            builder.Services.AddTransient(interfaceType, s =>
            {
                var httpClientFactory = s.GetRequiredService<IHttpClientFactory>();
                var httpClient = httpClientFactory.CreateClient(builder.Name);

                return RestService.For(interfaceType, httpClient, settings);
            });

            return builder;
        }

        public static IServiceCollection AddClients(this IServiceCollection services, string baseUrl, Assembly assembly, Action<IHttpClientBuilder, Type>configure = null)
        {
            foreach (var type in assembly.ExportedTypes.Where(x => x.IsInterface))
            {
                var attr = type.GetCustomAttribute<ApiClientAttribute>();
                if (attr != null)
                {
                    var builder = AddClient(services, type, baseUrl);
                    configure?.Invoke(builder, type);
                }
            }

            return services;
        }

        public static IServiceCollection AddClients(this IServiceCollection services, string baseUrl, string assemblyName, Action<IHttpClientBuilder, Type> configure = null)
        {            
            return AddClients(services, baseUrl, Assembly.LoadFrom(assemblyName), configure);
        }
    }
}
