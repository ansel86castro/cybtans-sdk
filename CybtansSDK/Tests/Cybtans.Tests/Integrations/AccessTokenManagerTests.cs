using Cybtans.AspNetCore;
using Cybtans.Refit;
using Cybtans.Serialization;
using Cybtans.Tests.Clients;
using Cybtans.Tests.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Cybtans.Tests.Integrations
{

    public class TestResponseHandler : DelegatingHandler
    {
        int _retry;
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            HttpResponseMessage response;
            if (request.Headers.Authorization != null && request.Headers.Authorization.Parameter == null)
            {
                response = new HttpResponseMessage(System.Net.HttpStatusCode.Forbidden)
                {
                    ReasonPhrase = "Access Token Expired",
                    RequestMessage = request,
                };             
            }
            else
            {
                _retry++;
                if (_retry >= 3)
                {
                    var data = new GetAllOrderStateResponse
                    {
                        Items = Enumerable.Range(1, 100).Select(x => new OrderStateDto { Id = x, Name = $"{x}" }).ToList(),
                        Page = 0,
                        TotalCount = 100,
                        TotalPages = 1
                    };
                    response = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
                    response.Content = new ByteArrayContent(BinaryConvert.Serialize(data));
                    response.Content.Headers.ContentType = new MediaTypeHeaderValue(BinarySerializer.MEDIA_TYPE);
                }
                else
                {
                    response = new HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized)
                    {
                        ReasonPhrase = "Access Token Expired",
                        RequestMessage = request,
                    };
                }
            }

            return Task.FromResult(response);
        }
    }

    public class AccessTokenManagerTests
    {
        private TokenManagerOptions _options;
        AccessTokenManager _tokenManager;
        IOrderStateServiceClient _service;

        public AccessTokenManagerTests()
        {
            var services = new ServiceCollection();

            AddTokenManager(services);

            AddClient<IOrderStateServiceClient>(services);

            var provider = services.BuildServiceProvider();
            _service = provider.GetRequiredService<IOrderStateServiceClient>();
        }

        private static void AddClient<T>(ServiceCollection services)
        {
            var interfaceType = typeof(T);

            var builder = services.AddClient(interfaceType, "http://localhost");

            builder.AddHttpMessageHandler<TokenManagerAuthenticationHandler>();
            builder.AddHttpMessageHandler(() => new TestResponseHandler());
        }

        private void AddTokenManager(ServiceCollection services)
        {
            var apiClient = new TestAccessTokenApiClient();
            _options = new TokenManagerOptions
            {
                ClientId = "TEST",
                ClientSecret = "TEST"
            };
            _tokenManager = new AccessTokenManager( _options, apiClient);

            services.AddSingleton<IAccessTokenManager>(_tokenManager);
            services.TryAddTransient<TokenManagerAuthenticationHandler>();
        }

        [Fact]
        public async Task ShouldSetAccessToken()
        {
            var response =   await  _service.GetAll(new Models.GetAllRequest());
            Assert.NotNull(response);
            Assert.NotEmpty(response.Items);
            Assert.Equal(100, response.Items.Count);
        }

        [Fact]
        public async Task TestAccessTokenApiClient()
        {
            var client = new AccessTokenApiClient("http://localhost/connect/token");
            var ex = Assert.ThrowsAsync<InvalidOperationException>(()=> 
                client.GetClientCredentialsTokenAsync("test", "test", "test"));

            Assert.NotNull(ex);
        }
    }
}
