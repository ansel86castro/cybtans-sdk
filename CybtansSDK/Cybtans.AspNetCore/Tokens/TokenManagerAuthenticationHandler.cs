using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace Cybtans.AspNetCore
{
  
    public class TokenManagerAuthenticationHandler: DelegatingHandler
    {
        private IAccessTokenManager _tokenManager;

        public TokenManagerAuthenticationHandler(IAccessTokenManager accessTokenManager)
        {
            _tokenManager = accessTokenManager;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // See if the request has an authorize header            
            if (request.Headers.Authorization != null)
            {
                var token = await _tokenManager.GetToken();
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                _tokenManager.ClearToken();
                throw new HttpRequestException("Request not Authorized");
            }

            return response;
        }
    }
}
