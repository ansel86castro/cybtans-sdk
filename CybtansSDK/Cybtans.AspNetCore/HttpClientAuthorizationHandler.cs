using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace Cybtans.AspNetCore
{
    public class AuthenticatedHttpClientHandler : DelegatingHandler
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public AuthenticatedHttpClientHandler(IHttpContextAccessor accessor)
        {
            _contextAccessor = accessor;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // See if the request has an authorize header            
            var auth = request.Headers.Authorization;
            if (auth != null)
            {
                if (_contextAccessor.HttpContext.Request.Headers.ContainsKey(HeaderNames.Authorization))
                {
                    var token = _contextAccessor.HttpContext.GetTokenAsync("access_token").Result;
                    request.Headers.Authorization = new AuthenticationHeaderValue(auth.Scheme, token);
                }
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
