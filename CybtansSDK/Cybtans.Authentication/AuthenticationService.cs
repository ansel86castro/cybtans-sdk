using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Cybtans.Authentication
{

    public class AuthenticationOptions
    {
        public string Issuer { get; set; } = "https://localhost:5001";

        public string Audience { get; set; } = "api";

        public bool EnableRefreshToken { get; set; }

        public long ExpirationTimeSeconds { get; set; } = 600;

        public long RefreshTokenExpireTimeSeconds { get; set; } = 1200;
    }

    public class AuthenticationService
    {
        private readonly ICertificateRepository _certificates;
        private readonly IPasswordAuthenticationHandler? _passwordHandler;
        private readonly IRefreshTokenAuthenticationHandler? _refreshTokenHandler;
        private readonly IClientCredentialAuthenticationHandler? _clientCredentialAuthenticationHandler;
        private readonly AuthenticationOptions _options;

        public AuthenticationService(
            AuthenticationOptions options,
            ICertificateRepository certificates,
            IPasswordAuthenticationHandler? passwordAuthenticationHandler = null,
            IRefreshTokenAuthenticationHandler? refreshTokenAuthenticationHandler = null,
            IClientCredentialAuthenticationHandler? clientCredentialAuthenticationHandler = null)
        {
            _options = options;
            _certificates = certificates;
            _passwordHandler = passwordAuthenticationHandler;
            _refreshTokenHandler = refreshTokenAuthenticationHandler;
            _clientCredentialAuthenticationHandler = clientCredentialAuthenticationHandler;
        }

        public async Task<List<JwtkModel>> GetJwtkSet()
        {
            var result = new List<JwtkModel>();
            foreach (var cert in await _certificates.GetCertificates().ConfigureAwait(false))
            {                
                var key = cert.Certificate.GetRSAPublicKey() ?? throw new InvalidOperationException($"No RSA PK was found in the certificate KeyId:{cert.KeyId}");
                var parameters = key.ExportParameters(false);
                var exponent = Base64UrlEncoder.Encode(parameters.Exponent);
                var modulus = Base64UrlEncoder.Encode(parameters.Modulus);
                
                result.Add(new()
                {
                    Kty = "RSA",
                    Alg = "RS256",
                    Kid = cert.KeyId,
                    Use = "sig",
                    E = exponent,
                    N = modulus,
                    X5t = cert.Certificate.Thumbprint
                });
            }

            return result;
            
        }

        public async Task<TokenResult> GetToken(TokenRequest request)
        {
            ClaimsIdentity? identity = request.GrandType switch
            {
                GrantTypes.Password => _passwordHandler != null ? await _passwordHandler.AuthenticateAsync(
                    username: request.Username ?? throw new ArgumentNullException(nameof(request.Username)),
                    password: request.Password ?? throw new ArgumentNullException(nameof(request.Password))) : throw new InvalidOperationException(),

                GrantTypes.RefreshToken => _refreshTokenHandler != null ? await _refreshTokenHandler.AuthenticateAsync(request.RefreshToken ?? throw new ArgumentNullException(nameof(request.RefreshToken)))
                : throw new InvalidOperationException(),
                GrantTypes.ClientCredentials => _clientCredentialAuthenticationHandler != null ? await _clientCredentialAuthenticationHandler.AuthenticateAsync(
                    request.ClientId ?? throw new ArgumentNullException("ClientId"), request.ClientSecret ?? throw new ArgumentNullException("ClientSecret"), request.Hint)
                : throw new InvalidOperationException(),
                _ => throw new ArgumentException("grand type not supported")
            };

            if (identity == null)
                throw new UnauthorizedAccessException();

            return await GetToken(identity, request.GrandType != GrantTypes.ClientCredentials);
        }

        private async Task<string> CreateRefreshToken(string userId,
          SigningCredentials credentials, JwtSecurityTokenHandler tokenHandler)
        {
            if (_refreshTokenHandler == null)
                throw new InvalidOperationException("RefreshTokenHandler is null");

            var expire = _options.RefreshTokenExpireTimeSeconds > 0 ?
                 DateTime.UtcNow.AddSeconds(_options.RefreshTokenExpireTimeSeconds) :
                 DateTime.UtcNow.AddYears(25);

            var header = new JwtHeader(credentials);           
            var payload = new JwtPayload();
            payload.AddClaim(new Claim(JwtRegisteredClaimNames.Iss, _options.Issuer));
            payload.AddClaim(new Claim(JwtRegisteredClaimNames.Sub, userId));
            payload.AddClaim(new Claim(JwtRegisteredClaimNames.Aud, _options.Audience));
            payload.AddClaim(new Claim(JwtRegisteredClaimNames.Exp, Math.Round((expire - EpochTime.UnixEpoch).TotalSeconds).ToString()));            
            
            var refreshToken = await _refreshTokenHandler.AddAsync(new()
            {
                UserId = userId,
                ExpireAt = expire,
                DeviceId = "Default"
            });

            payload.AddClaim(new Claim("rtid", refreshToken.Id));
            var token = new JwtSecurityToken(header, payload);
            return tokenHandler.WriteToken(token);

        }

        public async Task<TokenResult> GetToken(ClaimsIdentity identity, bool addRefreshToken)
        {
            var cert = await _certificates.GetDefaultCertificate() ?? throw new InvalidOperationException("Default Certificate not found");
            var credentials = new X509SigningCredentials(cert.Certificate, SecurityAlgorithms.RsaSha256);            

            var expire = _options.ExpirationTimeSeconds > 0 ?
                DateTime.UtcNow.AddSeconds(_options.ExpirationTimeSeconds) :
                DateTime.UtcNow.AddYears(25);

            var token = new JwtSecurityToken(
                issuer: _options.Issuer,
                audience: _options.Audience,
                claims: identity.Claims,
                expires: expire,
                signingCredentials: credentials);

            var jwt = new JwtSecurityTokenHandler();
            var reply = new TokenResult
            {
                AccessToken = jwt.WriteToken(token),
                ExpiresIn = (int)TimeSpan.FromDays(1).TotalSeconds,
                TokenType = "Bearer",
            };

            if (_refreshTokenHandler != null && addRefreshToken && _options.EnableRefreshToken)
            {
                var userId = identity.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
                if (userId != null)
                {
                    reply.RefreshToken = await CreateRefreshToken(userId, credentials, jwt);
                }
            }

            return reply;
        }
    }
}
