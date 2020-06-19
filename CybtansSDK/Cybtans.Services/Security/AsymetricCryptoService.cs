using System.Security.Cryptography;
using System.Text;

namespace Cybtans.Services.Security
{
    public class AsymetricCryptoService : CryptoService
    {
        public AsymetricCryptoService()
        {

        }

        public AsymetricCryptoService(int keySize)
        {
            KeySize = keySize;
        }

        public AsymetricCryptoService(string key, int keySize = 2048)
        {
            Key = key;
            KeySize = keySize;
        }

        public string Key { get; set; }

        public int KeySize { get; set; } = 2048;

        public void GenerateKeys(out string publicKey, out string privateKey, int keySize = 2048)
        {
            RSAParameters privateKeyInfo;
            RSAParameters publicKeyInfo;

            // Create a new instance of DSACryptoServiceProvider to generate 
            // a new key pair. 
            using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider(keySize))
            {
                privateKeyInfo = RSA.ExportParameters(true);
                publicKeyInfo = RSA.ExportParameters(false);

                privateKey = RSA.ToXmlString(true);
                publicKey = RSA.ToXmlString(false);
            }
        }

        public override byte[] Encrypt(byte[] source)
        {
            byte[] encryptedData;
            //Create a new instance of RSACryptoServiceProvider.
            using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider(KeySize))
            {

                //Import the RSA Key information. This only needs
                //toinclude the public key information.
                RSA.FromXmlString(Key);

                //Encrypt the passed byte array and specify OAEP padding.  
                //OAEP padding is only available on Microsoft Windows XP or
                //later.  
                encryptedData = RSA.Encrypt(source, false);
            }
            return encryptedData;
        }

        public override byte[] Decrypt(byte[] source)
        {
            byte[] decryptedData;
            //Create a new instance of RSACryptoServiceProvider.
            using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider(KeySize))
            {

                //Import the RSA Key information. This only needs
                //toinclude the public key information.
                RSA.FromXmlString(Key);

                //Encrypt the passed byte array and specify OAEP padding.  
                //OAEP padding is only available on Microsoft Windows XP or
                //later.  
                decryptedData = RSA.Decrypt(source, false);
            }

            return decryptedData;
        }

        public override string Encrypt(string source)
        {
            var bytes = Encrypt(Encoding.UTF8.GetBytes(source));
            return Encoding.Default.GetString(bytes);
        }

        public override string Decrypt(string source)
        {
            var bytes = Decrypt(Encoding.UTF8.GetBytes(source));
            return Encoding.Default.GetString(bytes);
        }

        public byte[] CreateSignature(byte[] data)
        {
            var hash = ComputeHash(data);

            return Encrypt(hash);
        }

        public bool CheckSignature(byte[] value, byte[] signature)
        {
            var valueHash = ComputeHash(value);
            var signatureHash = Decrypt(signature);
            if (valueHash.Length != signatureHash.Length)
            {
                return false;
            }

            for (int i = 0; i < valueHash.Length; i++)
            {
                if (valueHash[i] != signatureHash[i])
                {
                    return false;
                }
            }

            return true;
        }

        public bool CheckSignature(string value, string signature)
        {
            return CheckSignature(Encoding.UTF8.GetBytes(value), Encoding.UTF8.GetBytes(signature));
        }


    }
}
