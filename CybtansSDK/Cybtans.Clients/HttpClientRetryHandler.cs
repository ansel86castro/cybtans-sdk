using Cybtans.Common;
using Polly;
using Polly.Retry;

namespace Cybtans.Clients
{
    public class HttpClientRetryHandler : DelegatingHandler
    {
        private ResiliencePipeline<HttpResponseMessage> _pipeline;

        public HttpClientRetryHandler()
        {
            _pipeline = new ResiliencePipelineBuilder<HttpResponseMessage>()
                .AddRetry(new RetryStrategyOptions<HttpResponseMessage>
                {
                    MaxRetryAttempts = 3,
                    Delay = TimeSpan.FromSeconds(2),
                    BackoffType = DelayBackoffType.Exponential
                })
                .Build();
        }
      

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = await _pipeline.ExecuteAsync(async ctx =>
               await base.SendAsync(request, ctx), cancellationToken).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                throw await ApiException.Create(request, response).ConfigureAwait(false);
            }

            return response;
        }
    }

    public class HttpVersionRequestHandler: DelegatingHandler
    {
        private readonly Version _version;

        public HttpVersionRequestHandler(Version version)
        {
            _version = version;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.Version = _version;
            return base.SendAsync(request, cancellationToken);
        }

        protected override HttpResponseMessage Send(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.Version = _version;
            return base.Send(request, cancellationToken);
        }
    }
}
