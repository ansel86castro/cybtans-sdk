using Cybtans.Serialization;
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
    public class TestHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = new HttpResponseMessage
            {
                Content = new ByteArrayContent(BinaryConvert.Serialize(new Customer
                {
                    Id = Guid.NewGuid(),
                    Name = ""
                })),
                StatusCode = System.Net.HttpStatusCode.OK
            };

            response.Content.Headers.ContentType = new MediaTypeHeaderValue(BinarySerializer.MEDIA_TYPE);            
            return Task.FromResult(response);
        }
    }
}
