using Cybtans.Services;
using Cybtans.Services.Auth;
using Cybtans.Services.Security;

namespace Cybtans.Authentication
{

    public class PasswordHasherOptions
    {
        public PasswordHasherOptions(string secret)
        {
            Secret = secret;
        }

        public PasswordHasherOptions() { Secret = Guid.NewGuid().ToString(); }
        public string Secret { get; set; }
    }


    public class PasswordHasher : IPasswordHasher
    {
        private readonly PasswordHasherOptions _options;

        public PasswordHasher(PasswordHasherOptions options)
        {
            _options = options;
        }

        public string HashPassword(string password)
        {
            SymetricCryptoService symetricCrypto = new SymetricCryptoService();
            return symetricCrypto.EncryptString(password, _options.Secret);
        }

        public bool VerifyPassword(string hashedPassword, string password)
        {
            var encrypted = HashPassword(password);
            return hashedPassword == encrypted;
        }
    }
}
