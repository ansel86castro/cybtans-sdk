using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Cybtans.Testing.Integration
{

    public class TestAuthenticationOptions : AuthenticationSchemeOptions
    {
        public Func<IEnumerable<Claim>> ClaimsProvider { get; set; }
    }

    public class TestAuthHandler : AuthenticationHandler<TestAuthenticationOptions>
    {
        public const string SCHEME = "TEST";

        public TestAuthHandler(IOptionsMonitor<TestAuthenticationOptions> options,
            ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            AuthenticateResult result;
            if (Request.Headers.TryGetValue(HeaderNames.Authorization, out var auth) && auth.ToString() == Scheme.Name)
            {
                var claims = Options.ClaimsProvider?.Invoke() ?? new[] { new Claim(ClaimTypes.Name, "TestUser") };
                var identity = new ClaimsIdentity(claims, SCHEME);
                var principal = new ClaimsPrincipal(identity);
                var ticket = new AuthenticationTicket(principal, Scheme.Name);

                result = AuthenticateResult.Success(ticket);
            }
            else
            {
                result = AuthenticateResult.NoResult();
            }

            return Task.FromResult(result);
        }
    }
}
