using Microsoft.IdentityModel.Tokens;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Cybtans.Authentication
{

    public class CertificateData : IDisposable
    {
        public required string KeyId {  get; init; }

        public required X509Certificate2 Certificate { get; init; }                    

        private static string ComputeHash(ReadOnlySpan<byte> data)
        {            
            var result = SHA1.HashData(data);
            StringBuilder sb = new();
            for (int i = 0; i < result.Length; i++)
            {
                sb.Append(result[i].ToString("x2"));
            }
            var hash_string = sb.ToString();
            return hash_string;
        }

        public static async Task<CertificateData> FromPEMFile(string crtFile, string keyFile)
        {
            var crtPEM = await File.ReadAllTextAsync(crtFile).ConfigureAwait(false);
            var keyPEM = await File.ReadAllTextAsync(keyFile);
            var cert = X509Certificate2.CreateFromPem(crtPEM, keyPEM);

            var keyIdExtension = cert.Extensions["2.5.29.14"] as X509SubjectKeyIdentifierExtension;            

            var obj = new CertificateData()
            {
                KeyId = keyIdExtension?.SubjectKeyIdentifier ?? ComputeHash(Encoding.UTF8.GetBytes(keyPEM)),
                Certificate = cert       
            };
            
            return obj;
        }

        public void Dispose()
        {
            Certificate.Dispose();
        }
    }

    public interface ICertificateRepository: IDisposable
    {      
        public ValueTask<ICollection<CertificateData>> GetCertificates();

        public ValueTask<CertificateData?> GetDefaultCertificate();
       
    }


    public abstract class LocalDirectoryRepository : ICertificateRepository
    {
        private const int CERTIFICATE_VALID_DAYS = 360;

        private DirectoryInfo _directory;
        List<CertificateData> _certificates = new();

        public LocalDirectoryRepository(string directory)
        {
            _directory = new DirectoryInfo(directory);
        }

        public DirectoryInfo Directory => _directory;
      

        public ValueTask<ICollection<CertificateData>> GetCertificates() => new ValueTask<ICollection<CertificateData>>(_certificates);

        public ValueTask<CertificateData?> GetDefaultCertificate() => new ValueTask<CertificateData?>(_certificates.FirstOrDefault());

        public async Task Initialize()
        {
            if (!_directory.Exists)
            {
                _directory.Create();
            }

            if (!_directory.GetFiles().Any())
            {
                await ExportCertificate(CreateClientCertificate("localhost"));
            }

            await LoadCertificates();
        }




        #region Private

        private async Task LoadCertificates()
        {
            var certs = new List<CertificateData>();
            foreach (var item in _directory.GetFiles())
            {
                if(item.Extension == ".crt")
                {
                    var name = Path.GetFileNameWithoutExtension(item.Name);
                    if (File.Exists(Path.Combine(_directory.FullName, $"{name}.key")))
                    {
                        certs.Add(await CertificateData.FromPEMFile(item.FullName, $"{item.DirectoryName}/{name}.key"));
                    }
                }
            }
            _certificates.Clear();
            _certificates.AddRange(certs);                
        }

        private X509Certificate2 CreateClientCertificate(string name)
        {
            X500DistinguishedName distinguishedName = new X500DistinguishedName($"CN={name}");

            using (RSA rsa = RSA.Create(2048))
            {
                var request = new CertificateRequest(distinguishedName, rsa, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);

                request.CertificateExtensions.Add(
                    new X509KeyUsageExtension(X509KeyUsageFlags.DataEncipherment | X509KeyUsageFlags.KeyEncipherment | X509KeyUsageFlags.DigitalSignature, false));

                request.CertificateExtensions.Add(
                    new X509EnhancedKeyUsageExtension(
                        new OidCollection { new Oid("1.3.6.1.5.5.7.3.2") }, false));

                request.CertificateExtensions.Add(new X509SubjectKeyIdentifierExtension(rsa.ExportSubjectPublicKeyInfo(), false));  

                return request.CreateSelfSigned(new DateTimeOffset(DateTime.UtcNow.AddDays(-1)), new DateTimeOffset(DateTime.UtcNow.AddDays(CERTIFICATE_VALID_DAYS)));
            }
        }


        private async Task ExportCertificate(X509Certificate2 cert)
        {
            var data = cert.ExportCertificatePem();

            if (!_directory.Exists)
                _directory.Create();

            var key = cert.GetRSAPrivateKey();
            if (key == null) throw new InvalidOperationException("The certificate does not contains a private key");
            string privKeyPem = key.ExportPkcs8PrivateKeyPem();

            await File.WriteAllTextAsync(Path.Combine(_directory.FullName, "cert.crt"), data);
            await File.WriteAllTextAsync(Path.Combine(_directory.FullName, "cert.key"), privKeyPem);            
        }

        public virtual void Dispose()
        {
            foreach (var item in _certificates)
            {
                item.Dispose();
            }

            GC.SuppressFinalize(this);
        }


        #endregion
    }


    public class XmlLocalDirectoryRepository : LocalDirectoryRepository, IDisposable
    {
        public class KeyPair
        {
            public string Id { get; set; }

            public string PublicKey { get; set; }

            public string PrivateKey { get; set; }
        }


        public enum KeyType
        {
            Public, Private
        }


        private Dictionary<string, KeyPair> _keys = new Dictionary<string, KeyPair>();
        private static readonly SemaphoreSlim _lock = new SemaphoreSlim(1);

        public XmlLocalDirectoryRepository(string directory): base(directory) { }
        

        public int KeySize => 2048;

        public void EnsureDirectory()
        {
            if (!Directory.Exists)
            {
                Directory.Create();
            }
        }

        public async Task<string> Get(string id, KeyType type)
        {
            await EnsureKeys(id);

            return type switch
            {
                KeyType.Public => _keys[id].PublicKey,
                KeyType.Private => _keys[id].PrivateKey,
                _ => throw new NotImplementedException(),
            };
        }

        public async Task Save(string id, string content, KeyType type)
        {
            switch (type)
            {
                case KeyType.Public:
                    {
                        await File.WriteAllTextAsync($"{Directory}/{id}-public.key", content);
                    }
                    break;
                case KeyType.Private:
                    {
                        await File.WriteAllTextAsync($"{Directory}/{id}-private.key", content);
                    }
                    break;
                default:
                    throw new InvalidOperationException();
            }
        }

        private async ValueTask EnsureKeys(string id)
        {
            if (_keys.ContainsKey(id)) return;

            await _lock.WaitAsync();

            try
            {
                if (_keys.ContainsKey(id)) return;                
                if (!File.Exists($"{Directory}/{id}-private.key"))
                {
                    EnsureDirectory();
                    _keys[id] =  await GenerateKeys(id);
                }
                else
                {
                    _keys[id] = new KeyPair
                    {
                        Id = id,
                        PublicKey = await File.ReadAllTextAsync($"{Directory}/{id}-public.key"),
                        PrivateKey = await File.ReadAllTextAsync($"{Directory}/{id}-private.key")
                    };                    
                }
            }
            finally
            {
                _lock.Release();
            }
        }

        private async Task<KeyPair> GenerateKeys(string id)
        {
            using (var rsa = new RSACryptoServiceProvider(KeySize))
            {
                var publicKeyXml = rsa.ToXmlString(false);
                await File.WriteAllTextAsync($"{Directory}/{id}-public.key", publicKeyXml);

                var privateKeyXml = rsa.ToXmlString(true);
                await File.WriteAllTextAsync($"{Directory}/{id}-private.key", privateKeyXml);

                return new KeyPair()
                {
                    Id = id,
                    PrivateKey = privateKeyXml,
                    PublicKey = publicKeyXml
                };
            }
        }

        public override void Dispose()
        {
            base.Dispose();
            _lock.Dispose();
        }    
    }
}

