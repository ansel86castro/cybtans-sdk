using Cybtans.Clients;
using Cybtans.Common;
using Cybtans.Serialization;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtension
    {
        public static IHttpClientBuilder AddClient<TContract, TImpl>(this IServiceCollection services)
            where TContract : class
            where TImpl : class
        {
            return AddClient(services, typeof(TContract), typeof(TImpl));
        }

        public static IHttpClientBuilder AddBinaryClient<TContract, TImpl>(this IServiceCollection services, string baseUrl)
            where TContract : class
            where TImpl : class
        {            
            return AddClient<TContract, TImpl>(services).ConfigureTransport(baseUrl);
        }

        public static IHttpClientBuilder AddClient(this IServiceCollection services, Type interfaceType, Type concreteType)
        {
            if (!interfaceType.IsAssignableFrom(concreteType))
                throw new InvalidOperationException($"{concreteType.FullName} does not implement {interfaceType.FullName}");

            var serviceName = interfaceType.FullName;                    
            var builder = services.AddHttpClient(serviceName);           
           
            builder.Services.TryAddSingleton(interfaceType, s =>
            {
                var httpClientFactory = s.GetRequiredService<IHttpClientFactory>();
                var httpClient = httpClientFactory.CreateClient(builder.Name);
                var serializer = s.GetService<IHttpContentSerializer>();
                return Activator.CreateInstance(concreteType, httpClient, serializer) ?? throw new InvalidOperationException($"Unable to create {concreteType.FullName}");
            });

            return builder;
        }

        public static IHttpClientBuilder AddClients(this IServiceCollection services, Assembly assembly)
        {            
            var builder = services.AddHttpClient(assembly.GetName().Name);         

            foreach (var type in assembly.ExportedTypes.Where(x => x.IsClass))
            {
                var attr = type.GetCustomAttribute<ApiClientAttribute>();
                if (attr != null)
                {
                    var interfaceType = type.GetInterfaces().FirstOrDefault();
                    if (interfaceType != null)
                    {
                        services.TryAddSingleton(interfaceType, s =>
                        {
                            var httpClientFactory = s.GetRequiredService<IHttpClientFactory>();
                            var httpClient = httpClientFactory.CreateClient(builder.Name);

                            var serializer = s.GetService<IHttpContentSerializer>();
                            return Activator.CreateInstance(type, httpClient, serializer) ?? throw new InvalidOperationException($"Unable to create {type.FullName}");
                        });
                    }
                }
            }

            return builder;
        }

        public static IHttpClientBuilder ConfigureTransport(this IHttpClientBuilder builder, string baseUrl, bool binary = true)
        {
            if (binary)
            {
                builder.Services.TryAddSingleton<IHttpContentSerializer, CybtansContentSerializer>();
            }

            builder.ConfigureHttpClient(c =>
            {
                c.BaseAddress = new Uri(baseUrl);
                if (binary)
                {
                    c.DefaultRequestHeaders.Add("Accept", $"{BinarySerializer.MEDIA_TYPE}; charset={BinarySerializer.DefaultEncoding.WebName}");
                }
            });
            builder.AddHttpMessageHandler(() => new HttpClientRetryHandler());
            return builder;
        }
     
    }
}
