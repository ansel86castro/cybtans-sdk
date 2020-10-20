using Cybtans.AspNetCore;
using System;
using System.Threading.Tasks;

namespace Cybtans.Tests.Integrations
{
    public class TestAccessTokenApiClient : IAccessTokenApiClient
    {
        public Task<string> GetClientCredentialsTokenAsync(string clientId, string clientSecret, string scope)
        {
            if (clientId == "TEST")
                return Task.FromResult("TEST");

            throw new InvalidOperationException("INVALID CLIENT");
        }
    }
}
