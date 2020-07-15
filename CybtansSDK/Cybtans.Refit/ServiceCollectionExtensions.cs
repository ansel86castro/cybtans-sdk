using Cybtans.Refit;
using Cybtans.Serialization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Refit;
using System;
using System.Text;

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
    }
}
