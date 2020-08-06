using Cybtans.Serialization;
using Cybtans.Services;
using Cybtans.Services.Extensions;
using Cybtans.Test.Domain;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

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
