using Cybtans.Serialization;
using Polly;
using Polly.Extensions.Http;
using Polly.Retry;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Cybtans.Refit
{
    public class HttpClientErrorHandler : DelegatingHandler
    {
        private AsyncRetryPolicy<HttpResponseMessage> _policy;

        public HttpClientErrorHandler()
        {
            _policy = HttpPolicyExtensions
              .HandleTransientHttpError()
              .WaitAndRetryAsync(new TimeSpan[]
                {
                    TimeSpan.FromSeconds(3),
                    TimeSpan.FromSeconds(5),
                    TimeSpan.FromSeconds(8),
                });
        }
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = await _policy.ExecuteAsync((ctx) => base.SendAsync(request, ctx), cancellationToken).ConfigureAwait(false);
            if (!response.IsSuccessStatusCode)
            {
                throw await ApiException.Create(request, response).ConfigureAwait(false);
            }

            return response;
        }
    }
}
