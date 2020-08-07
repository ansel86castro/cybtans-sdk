using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace Cybtans.Testing.Integration
{
    //public class TestHttpClientAuthenticationOptions
    //{
    //    public string Scheme { get; set; }

    //    public string Key { get; set; }

    //    public string Issuer { get; set; } = "www.test.com";

    //    public string Audience { get; set; } = "test";

    //    public int ExpireTimeInMinutes { get; set; } = 120;

    //    public Func<IEnumerable<Claim>> ClaimsProvider { get; set; }
    //}

    public class TestHttpClientAuthenticationHandler : DelegatingHandler
    {

        //TestHttpClientAuthenticationOptions _options;

        //public TestHttpClientAuthenticationHandler(TestHttpClientAuthenticationOptions options)
        //{
        //    _options = options;
        //}

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // See if the request has an authorize header            
            var auth = request.Headers.Authorization;
            if (auth != null)
            {
                if (request.Headers.Authorization != null)
                {
                    request.Headers.Authorization = new AuthenticationHeaderValue(TestAuthHandler.SCHEME);
                }
            }
            return base.SendAsync(request, cancellationToken);
        }

        //private string GenerateAccessToken()
        //{
        //    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Key));
        //    var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        //    var claims = _options.ClaimsProvider?.Invoke() ?? new Claim[0];

        //    var token = new JwtSecurityToken(_options.Issuer,
        //        _options.Audience,
        //        claims,
        //        expires: DateTime.Now.AddMinutes(_options.ExpireTimeInMinutes),
        //        signingCredentials: credentials);

        //    return new JwtSecurityTokenHandler().WriteToken(token);
        //}
    }
}
