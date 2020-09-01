using Cybtans.Services;
using Cybtans.Tests.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
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

    }

    [RegisterDependency(typeof(IAuthenticationService))]
    public class AuthenticationService : IAuthenticationService
    {
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
                new Claim(ClaimTypes.Role, user.Roles)
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
        public Task<LoginResponse> Login(LoginRequest request)
        {
            var user = _users.FirstOrDefault(x => x.Username == request.Username && x.Password == request.Password);
            if(user == null)
            {
                throw new ValidationException("User credentials not valid");
            }

            var token = GenerateAccessToken(user);
            return Task.FromResult(new LoginResponse
            {
                Token = token
            });
        }
    }
}
