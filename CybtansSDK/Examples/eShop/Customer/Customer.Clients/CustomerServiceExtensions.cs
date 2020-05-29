using System;
using Refit;
using Cybtans.Refit;
using Cybtans.Serialization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Text;


namespace Customer.Clients
{
	
	public class CustomerServiceOption
	{
	    public string BaseUrl { get; set; }
	}
	
	public static class CustomerServiceExtensions
	{
	    public static void AddCustomerService(this IServiceCollection services, IConfiguration configuration, RefitSettings settings = null)
	    {
	        var option = configuration.GetSection("CustomerService").Get<CustomerServiceOption>();
	
	        if(settings == null)
	        {
	            settings = new RefitSettings();
	        }
	
	        settings.ContentSerializer = new CybtansContentSerializer(settings.ContentSerializer);
	
	        var builder = services.AddRefitClient<ICustomerService>(settings);
	
	        builder.ConfigureHttpClient(c =>
	        {                
	            c.BaseAddress = new Uri(option.BaseUrl);
	            c.DefaultRequestHeaders.Add("Accept", $"{BinarySerializer.MEDIA_TYPE}; charset={Encoding.UTF8.WebName}");
	        });
	    }       
	}

}
