using Cybtans.Refit;
using Cybtans.Serialization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Refit;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;
using Xunit;
using System.Threading.Tasks;
using IdentityModel;
using Cybtans.Testing.Integration;
using Microsoft.AspNetCore.TestHost;
using System;

namespace Cybtans.Tests.Integrations
{
    public class BaseIntegrationFixture<T>: WebApplicationFactory<T>, IAsyncLifetime
        where T:class
    {
        private Action<IServiceCollection> _configureServices;

        public IEnumerable<Claim> Claims { get; set; }       

        public BaseIntegrationFixture()
        {
            Claims = new Claim[]
            {
                new Claim(JwtClaimTypes.Subject, "admin"),
                new Claim(JwtClaimTypes.Name, "admin"),
                new Claim(ClaimTypes.Email, "admin@gmail.com")
            };
        }

        public HttpClient Client { get; private set; }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {                               
                ConfigureServices(services);

                _configureServices?.Invoke(services);
            });
          
        }

        public virtual void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(TestAuthHandler.SCHEME)
               .AddScheme<TestAuthenticationOptions, TestAuthHandler>(TestAuthHandler.SCHEME, options =>
               {
                   options.ClaimsProvider = () => Claims;
               });          
        }
                 
        
        protected override void ConfigureClient(HttpClient client)
        {           
            client.DefaultRequestHeaders.Add("Accept", $"{BinarySerializer.MEDIA_TYPE}; charset={Encoding.UTF8.WebName}");           
            base.ConfigureClient(client);
        }
             
                   
        public TClient GetClient<TClient>(HttpClient httpClient = null)
          where TClient : class
        {            
            httpClient ??= Client;
            var settings = new RefitSettings();
            settings.ContentSerializer = new CybtansContentSerializer(settings.ContentSerializer);
            return RestService.For<TClient>(httpClient, settings);
        }
            

        public virtual Task InitializeAsync()
        {           
            Client = this.CreateHttpClient();
            return Task.CompletedTask;
        }


        public virtual Task DisposeAsync()
        {
            return Task.CompletedTask;
        }

      
        public TestHostPipeline CreatePipeline()        
        {
            return new TestHostPipeline((BaseIntegrationFixture<T>)Activator.CreateInstance(GetType()));
        }


        public class TestHostPipeline
        {
            BaseIntegrationFixture<T> _fixture;

            public TestHostPipeline(BaseIntegrationFixture<T> fixture)
            {
                _fixture = fixture;
            }

            public TestHostPipeline WithClaims(IEnumerable<Claim> claims)
            {
                _fixture.Claims = claims;
                return this;
            }

            public TestHostPipeline ConfigureServices(Action<IServiceCollection> configureServices)
            {
                _fixture._configureServices = configureServices;
                return this;
            }

            public async Task Run(Func<BaseIntegrationFixture<T>, Task> func)
            {
                try
                {
                    await _fixture.InitializeAsync();
                    try
                    {
                        await func(_fixture);
                    }
                    finally
                    {
                        await _fixture.DisposeAsync();
                    }
                }
                finally
                {
                    _fixture.Dispose();
                }
            }
        }
    }

    public static class WebApplicationFactoryExtensions
    {
        public static HttpClient CreateHttpClient<TStartup>(this WebApplicationFactory<TStartup> factory)
            where TStartup : class
        {
            return factory.CreateDefaultClient(
                new TestHttpClientAuthenticationHandler(),
                new HttpClientErrorHandler());
        }

        public static TClient GetClient<TClient, TStartup>(this WebApplicationFactory<TStartup> factory) 
            where TClient : class
            where TStartup : class
        {
            var httpClient = factory.CreateHttpClient();
            var settings = new RefitSettings();
            settings.ContentSerializer = new CybtansContentSerializer(settings.ContentSerializer);
            return RestService.For<TClient>(httpClient, settings);
        }

      
    }
}
