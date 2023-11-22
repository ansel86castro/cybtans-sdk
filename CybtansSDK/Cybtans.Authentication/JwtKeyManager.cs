//using Microsoft.Extensions.Caching.Memory;
//using Microsoft.IdentityModel.Tokens;
//using System.Security.Cryptography;
//using System.Security.Cryptography.X509Certificates;

//namespace Cybtans.Authentication
//{ 

//    public class JwtKeyManager : IJwtKeyManager
//    {                
//        private readonly ICertificateRepository _jwtkRepository;

//        private CertificateData? certData;
//        private X509Certificate2? cert;


//        public JwtKeyManager(ICertificateRepository jwtkRepository)
//        {
//            _jwtkRepository = jwtkRepository;       
//        }

//        public async Task<JwtkModel> GetJwtk()
//        {
//            var cert = await GetSigningCertificate();
//            var key = cert.GetRSAPublicKey() ?? throw new InvalidOperationException("No RSA PK was found in the default certificate");           
//            var parameters = key.ExportParameters(false);
//            var exponent = Base64UrlEncoder.Encode(parameters.Exponent);
//            var modulus = Base64UrlEncoder.Encode(parameters.Modulus);

//            return new JwtkModel
//            {
//                Kty = "RSA",
//                Alg = "RS256",
//                Kid = KeyId,
//                Use = "sig",
//                E = exponent,
//                N = modulus               
//            };
//        }

//        public async Task<RsaSecurityKey> GetPrivateRsaSecurityKey()
//        {
//            var cert = await GetSigningCertificate();
//            return new RsaSecurityKey(cert.GetRSAPrivateKey() ?? throw new InvalidOperationException("RSA private key not found in the default certificate")) { KeyId =  };            
//        }

//        public async Task<RsaSecurityKey> GetPublicRsaSecurityKey()
//        {         
//            var provider = new RSACryptoServiceProvider(_jwtkRepository.KeySize);
//            provider.FromXmlString(await _jwtkRepository.Get(KeyId, KeyType.Public));            

//            return new RsaSecurityKey(provider) { KeyId = KeyId };
//        }
          
//        public async ValueTask<X509Certificate2> GetSigningCertificate()
//        {
//            var certData = await _jwtkRepository.GetDefaultCertificate() ?? throw new InvalidOperationException("Default Certificate not found");
//            return X509Certificate2.CreateFromPem(certData.CertPEM, certData.KeyPEM);

//        }
//    }
//}
