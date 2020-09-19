using Cybtans.Serialization;
using Cybtans.Services;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Cybtans.Tests
{
    public class ErrorHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = await base.SendAsync(request, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                if (response.Content != null)
                {
                    var bytes = await response.Content.ReadAsByteArrayAsync();
                    var errorInfo = BinaryConvert.Deserialize<ValidationResult>(bytes);
                    throw new ValidationException(errorInfo);
                }
            }

            return response;
        }
    }
}
