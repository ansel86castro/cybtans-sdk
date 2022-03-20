using Cybtans.Tests.Services;
using IdentityModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Security.Cryptography;

namespace Cybtans.Tests.RestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JwksController : ControllerBase
    {
        public JwksController()
        {

        }

        [HttpGet]
        public IActionResult Get()
        {
            var key = GetVerificationKeys();
            return Ok(new
            {
                keys = new Jwtk[]
                {
                    key
                }
            });
        }

        private Jwtk GetVerificationKeys()
        {
            var key = AuthenticationService.GetPublicRsaSecurityKey();

            var parameters = key.Rsa.ExportParameters(false);
            var exponent = Base64Url.Encode(parameters.Exponent);
            var modulus = Base64Url.Encode(parameters.Modulus);

            var webKey = new Jwtk
            {
                kty = "RSA",
                use = "sig",
                kid = "6BB734F9-F9F5-4C90-ADB8-CC26E95CC452",
                e = exponent,
                n = modulus,
                alg = "sig"
            };

            return webKey;
        }

    }

    public class Jwtk
    {
        public string kty { get; set; }

        public string use { get; set; }

        public string kid { get; set; }

        public string e { get; set; }

        public string n { get; set; }

        public string alg { get; set; }

        public string x5t { get; set; }

        public string[] x5c { get; set; }
    }
}
