using System;
using System.Threading;
using System.Threading.Tasks;

namespace Cybtans.AspNetCore
{

    public class AccessTokenManager : IAccessTokenManager
    {
        private volatile string token;
        private TokenManagerOptions _options;
        private int _authenticate = 0;
        private TaskCompletionSource<string> _tcs;
        IAccessTokenApiClient _client;

        public AccessTokenManager(TokenManagerOptions options, IAccessTokenApiClient client)
        {
            _options = options;
            _client = client;
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
                        token = await _client.GetClientCredentialsTokenAsync(_options.ClientId, _options.ClientId, _options.Scope);
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
            Interlocked.Exchange(ref _authenticate, 0);
            Interlocked.Exchange(ref token, null);          
        }
    }   
}
