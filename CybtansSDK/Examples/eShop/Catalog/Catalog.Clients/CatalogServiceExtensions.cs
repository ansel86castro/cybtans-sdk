using System;
using Refit;
using Cybtans.Refit;
using Cybtans.Serialization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Text;


namespace Catalog.Clients
{
	
	public class CatalogServiceOption
	{
	    public string BaseUrl { get; set; }
	}
	
	public static class CatalogServiceExtensions
	{
	    public static void AddCatalogService(this IServiceCollection services, IConfiguration configuration, RefitSettings settings = null)
	    {
	        var option = configuration.GetSection("CatalogService").Get<CatalogServiceOption>();
	
	        if(settings == null)
	        {
	            settings = new RefitSettings();
	        }
	
	        settings.ContentSerializer = new CybtansContentSerializer(settings.ContentSerializer);
	
	        var builder = services.AddRefitClient<ICatalogService>(settings);
	
	        builder.ConfigureHttpClient(c =>
	        {                
	            c.BaseAddress = new Uri(option.BaseUrl);
	            c.DefaultRequestHeaders.Add("Accept", $"{BinarySerializer.MEDIA_TYPE}; charset={Encoding.UTF8.WebName}");
	        });
	    }       
	}

}
