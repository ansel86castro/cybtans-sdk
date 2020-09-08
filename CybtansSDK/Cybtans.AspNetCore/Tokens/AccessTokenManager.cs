using IdentityModel.Client;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Cybtans.AspNetCore
{

    public class AccessTokenManager : IAccessTokenManager
    {
        private volatile string token;
        private TokenManagerOptions _options;
        private int _authenticate =0;
        private TaskCompletionSource<string> _tcs;

        public AccessTokenManager(TokenManagerOptions options)
        {
            _options = options;
        }

        public async ValueTask<string> GetToken()
        {
            if (token == null)
            {
                if (Interlocked.CompareExchange(ref _authenticate, 1, 0) == 0)
                {
                    _tcs = new TaskCompletionSource<string>();
                    try
                    {
                        token = await FetchToken();
                        _tcs.SetResult(token);
                    }
                    catch (Exception e)
                    {
                        _tcs.SetException(e);

                        if (e is AggregateException ag && ag.InnerExceptions.Count == 1)
                            throw e.InnerException;

                        throw;
                    }
                }
                else
                {
                    while (_tcs == null) { Thread.SpinWait(5); }

                    await _tcs.Task;
                }

            }

            return token;
        }

        public void ClearToken()
        {
            token = null;
        }


        private async Task<string> FetchToken()
        {
            var client = new HttpClient();
            var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = _options.TokenEndpoint,
                ClientId = _options.ClientId,
                ClientSecret = _options.ClientSecret,
                Scope = _options.Scope
            });           

            if (tokenResponse.IsError)
            {
                throw new InvalidOperationException(tokenResponse.Error);
            }

            return tokenResponse.AccessToken;
        }
    }   
}
