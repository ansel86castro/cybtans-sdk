using Cybtans.Serialization;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Cybtans.Refit
{
    public class HttpClientExceptionHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
            if (!response.IsSuccessStatusCode)
            {
                throw await ApiException.Create(request, response).ConfigureAwait(false);
            }

            return response;
        }
    }
}
