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
using System.Linq;
using System.Reflection;

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

            builder.ConfigureTestServices(services =>
            {
                OnPostConfigureService(services);
            });
        }

        protected virtual void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(TestAuthHandler.SCHEME)
               .AddScheme<TestAuthenticationOptions, TestAuthHandler>(TestAuthHandler.SCHEME, options =>
               {
                   options.ClaimsProvider = () => Claims;
               });            
        }

        protected virtual void OnPostConfigureService(IServiceCollection services)
        {

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

      
        public TestHostPipeline CreateTest()        
        {
            return new TestHostPipeline(GetType());
        }


        public class TestHostPipeline
        {

            Type _fixtureType;
            List<Action<IServiceCollection>> _setups = new List<Action<IServiceCollection>>();         
            List<Claim> _claims = new List<Claim>();

            internal TestHostPipeline(Type fixtureType)
            {
                _fixtureType = fixtureType;                
            }

            private void OnConfigureService(IServiceCollection services)
            {
                _setups.ForEach(x => x(services));
            }

            private void SetupClaims(BaseIntegrationFixture<T> fixture)
            {
                if (fixture.Claims != null)
                {
                    var values = fixture.Claims.Where(x => !_claims.Any(y => y.Type == x.Type)).ToList();
                    values.AddRange(_claims);

                    fixture.Claims = values;
                }
                else
                {
                    fixture.Claims = _claims;
                }
            }


            public TestHostPipeline UseClaims(params Claim[] claims)
            {
                _claims.AddRange(claims);
                return this;
            }

            public TestHostPipeline UseRoles(params string[] roles)
            {
                return UseClaims(roles.Select(x => new Claim(ClaimTypes.Role, x)).ToArray());
            }

            public TestHostPipeline UseUser(string id, string name)
            {
                return UseClaims(
                      new Claim(ClaimTypes.NameIdentifier, id),
                      new Claim(ClaimTypes.Name, name)
                    );
            }

            public TestHostPipeline ConfigureServices(Action<IServiceCollection> configureServices)
            {
                _setups.Add(configureServices);
                return this;
            }

            public TestHostPipeline UseService<TService, TImplementation>()
                where TService: class
                where TImplementation : class
            {
                _setups.Add(services =>
                {                    
                    var descriptor = services.FirstOrDefault(x => x.ServiceType == typeof(TService));
                    if (descriptor != null)
                    {
                        services.Remove(descriptor);                        
                    }
                    services.AddTransient<TImplementation>();
                });
                return this;
            }

            public TestHostPipeline UseService<TService>(TService implementation)
                where TService : class                
            {
                _setups.Add(services =>
                {
                    var descriptor = services.FirstOrDefault(x => x.ServiceType == typeof(TService));
                    if (descriptor != null)
                    {
                        services.Remove(descriptor);                        
                    }

                    services.AddSingleton(implementation);
                });
                return this;
            }

            public TestHostPipeline UseService<TService, TImplementation>(Func<IServiceProvider, TImplementation> factory)
               where TService : class
               where TImplementation : class
            {
                _setups.Add(services =>
                {
                    var descriptor = services.FirstOrDefault(x => x.ServiceType == typeof(TService));
                    if (descriptor != null)
                    {
                        services.Remove(descriptor);                        
                    }
                    services.AddTransient(factory);
                });
                return this;
            }

            public async Task RunAsync(Func<BaseIntegrationFixture<T>, Task> func)
            {
                var fixture = (BaseIntegrationFixture<T>)Activator.CreateInstance(_fixtureType);
                SetupClaims(fixture);
                fixture._configureServices = OnConfigureService;

                try
                {
                    await fixture.InitializeAsync();
                    try
                    {
                        await func(fixture);
                    }
                    finally
                    {
                        await fixture.DisposeAsync();
                    }
                }
                finally
                {
                    fixture.Dispose();                   
                }
            }

            public void Run(Func<BaseIntegrationFixture<T>, Task> func)
            {
                RunAsync(func).Wait();
            }

            public Task RunAsync<TClient>(Func<TClient, Task> func)
                where TClient : class
            {
                return RunAsync(test =>
                {
                    var client = test.GetClient<TClient>();
                    return func(client);
                });
            }

            public void Run<TClient>(Func<TClient, Task> func)
                 where TClient : class
            {
                RunAsync<TClient>(func).Wait();
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
                new HttpClientExceptionHandler());
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
