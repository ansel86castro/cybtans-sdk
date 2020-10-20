using IdentityModel.Client;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Cybtans.AspNetCore
{
    public interface IAccessTokenApiClient
    {
        Task<string> GetClientCredentialsTokenAsync(string clientId, string clientSecret, string scope);        
    }

    

    public class AccessTokenApiClient : IAccessTokenApiClient
    {
        string _endpoint;
        public AccessTokenApiClient(string endpoint)
        {
            _endpoint = endpoint;
        }

        public async Task<string> GetClientCredentialsTokenAsync(string clientId, string clientSecret, string scope)
        {
            var client = new HttpClient();
            var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = _endpoint,
                ClientId = clientId,
                ClientSecret = clientSecret,
                Scope = scope
            });

            if (tokenResponse.IsError)
            {
                throw new InvalidOperationException(tokenResponse.Error);
            }

            return tokenResponse.AccessToken;
        }
    }
}
