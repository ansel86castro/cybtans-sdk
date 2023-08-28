using Cybtans.Common;
using Polly;
using Polly.Extensions.Http;
using Polly.Retry;

namespace Cybtans.Clients
{
    public class HttpClientRetryHandler : DelegatingHandler
    {
        private AsyncRetryPolicy<HttpResponseMessage> _policy;
        private Random rand = new Random();

        public HttpClientRetryHandler()
        {
            _policy = HttpPolicyExtensions
              .HandleTransientHttpError()
              .WaitAndRetryAsync(3, retry => TimeSpan.FromSeconds(Math.Pow(2, retry) + Jitter()))
              ;
        }

        private double Jitter() => rand.NextDouble();

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = await _policy.ExecuteAsync(ctx =>
                base.SendAsync(request, ctx), cancellationToken).ConfigureAwait(false);

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
