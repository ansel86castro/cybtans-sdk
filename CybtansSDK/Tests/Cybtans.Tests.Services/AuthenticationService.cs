using Cybtans.Services;
using Cybtans.Tests.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Cybtans.Tests.Services
{
    public class RegisteredUsers
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public string Roles { get; set; }
    }


    public class JwtOptions
    {     
  
        public string Issuer { get; set; }

        public string Audience { get; set; }

        public string Secret { get; set; }

        public string PublicKey { get; set; }

        public string PrivateKey { get; set; }

    }

    [RegisterDependency(typeof(IAuthenticationService))]
    public class AuthenticationService : IAuthenticationService
    {
        public const string KeyId = "6BB734F9-F9F5-4C90-ADB8-CC26E95CC452";
        private readonly JwtOptions _options;
        private readonly List<RegisteredUsers> _users;

        public AuthenticationService(IOptions<JwtOptions> options)
        {
            _options = options.Value;

            _users = new List<RegisteredUsers>
            {
                new RegisteredUsers
                {
                     Id = Guid.NewGuid().ToString(),
                     Username = "admin",
                     Password = "admin",
                     Roles = "admin"
                },
                new RegisteredUsers
                {
                     Id = Guid.NewGuid().ToString(),
                     Username = "test",
                     Password = "test",
                     Roles = "test"
                }
            };
        }

        private string GenerateAccessToken(RegisteredUsers user)
        {
           var claims = new Claim[]
           {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),               
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Roles),
                new Claim("client_id", "D6E29710-B68F-4D2D-9471-273DECF9C4B7"),
                new Claim("creator_id", "1")
           };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Secret));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_options.Issuer,
                _options.Audience,
                claims,
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string GenerateAccessTokenAsymetric(RegisteredUsers user)
        {
            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Roles),
                new Claim("client_id", "D6E29710-B68F-4D2D-9471-273DECF9C4B7"),
                new Claim("creator_id", "1")
            };

            var securityKey = GetRsaSecurityKey();
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.RsaSha256);

            var token = new JwtSecurityToken(_options.Issuer,
                _options.Audience,
                claims,
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: credentials);

            var jwt = new JwtSecurityTokenHandler();
            
            return jwt.WriteToken(token); 
        }

        public Task<LoginResponse> Login(LoginRequest request)
        {
            var user = _users.FirstOrDefault(x => x.Username == request.Username && x.Password == request.Password);
            if(user == null)
            {
                throw new ValidationException("User credentials not valid");
            }

            var token = GenerateAccessTokenAsymetric(user);
            return Task.FromResult(new LoginResponse
            {
                Token = token
            });
        }
   
        private RsaSecurityKey GetRsaSecurityKey()
        {          
            var provider = RSA.Create();
            var xml = File.ReadAllText("keys/private.key");            
            provider.FromXmlString(xml);

            return new RsaSecurityKey(provider) { KeyId = KeyId };
        }
        
      

        public static RsaSecurityKey GetPublicRsaSecurityKey()
        {
            var provider = RSA.Create();
            var pem = File.ReadAllText("keys/public.key");
            var xml = RsaKeyConverter.PemToXml(pem);
            provider.FromXmlString(xml);

            return new RsaSecurityKey(provider) { KeyId = AuthenticationService.KeyId };
        }
      
    }
}
