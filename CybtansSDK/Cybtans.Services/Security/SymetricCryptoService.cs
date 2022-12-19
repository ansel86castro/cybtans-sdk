using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;

namespace Cybtans.Services.Security
{
    public class SymetricCryptoService : CryptoService, ISymetricCryptoService
    {
        public byte[] Key { get; set; }

        public byte[] IV { get; set; }

        public int KeySize { get; set; }

        public void GenerateParameters()
        {
            using (RijndaelManaged myRijndael = new RijndaelManaged())
            {
                myRijndael.GenerateKey();

                myRijndael.GenerateIV();

                Key = myRijndael.Key;
                IV = myRijndael.IV;
                KeySize = myRijndael.KeySize;
            }
        }

        public override byte[] Encrypt(byte[] source)
        {

            using (RijndaelManaged myRijndael = new RijndaelManaged())
            {
                if (Key == null)
                {
                    myRijndael.GenerateKey();
                    Key = myRijndael.Key;
                    KeySize = myRijndael.KeySize;
                }
                else
                {
                    myRijndael.Key = Key;
                }

                if (IV == null)
                {
                    myRijndael.GenerateIV();
                    IV = myRijndael.IV;
                }
                else
                {
                    myRijndael.IV = IV;
                }

                // Encrypt the string to an array of bytes.
                // Create a decrytor to perform the stream transform.
                ICryptoTransform encryptor = myRijndael.CreateEncryptor(myRijndael.Key, myRijndael.IV);

                byte[] encrypted;
                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        csEncrypt.Write(source, 0, source.Length);
                        csEncrypt.FlushFinalBlock();
                        encrypted = msEncrypt.ToArray();
                    }
                }

                return encrypted;
            }
        }

        public override string Decrypt(string source)
        {
            string plaintext = null;
            var cipherText = Encoding.Unicode.GetBytes(source);
            // Create an RijndaelManaged object
            // with the specified key and IV.
            using (RijndaelManaged rijAlg = new RijndaelManaged())
            {
                rijAlg.Key = Key;
                rijAlg.IV = IV;

                // Create a decrytor to perform the stream transform.
                ICryptoTransform decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);

                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {

                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }

            }

            return plaintext;

        }

        public override byte[] Decrypt(byte[] source)
        {
            using (RijndaelManaged myRijndael = new RijndaelManaged())
            {
                myRijndael.Key = Key;
                myRijndael.IV = IV;

                ICryptoTransform decryptor = myRijndael.CreateDecryptor(myRijndael.Key, myRijndael.IV);

                byte[] decrypted;
                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(source))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (MemoryStream outputStream = new MemoryStream())
                        {
                            byte[] buffer = new byte[1024];
                            for (int bytes = csDecrypt.Read(buffer, 0, buffer.Length); bytes > 0; bytes = csDecrypt.Read(buffer, 0, buffer.Length))
                            {
                                outputStream.Write(buffer, 0, bytes);
                            }
                            decrypted = outputStream.ToArray();
                        }
                    }
                }

                return decrypted;
            }
        }

        public override string Encrypt(string source)
        {
            byte[] encrypted;
            // Create an RijndaelManaged object
            // with the specified key and IV.
            using (RijndaelManaged rijAlg = new RijndaelManaged())
            {
                if (Key == null)
                {
                    rijAlg.GenerateKey();
                    Key = rijAlg.Key;
                    KeySize = rijAlg.KeySize;
                }
                else
                {
                    rijAlg.Key = Key;
                }

                if (IV == null)
                {
                    rijAlg.GenerateIV();
                    IV = rijAlg.IV;
                }
                else
                {
                    rijAlg.IV = IV;
                }


                // Create a decrytor to perform the stream transform.
                ICryptoTransform encryptor = rijAlg.CreateEncryptor(rijAlg.Key, rijAlg.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {

                            //Write all data to the stream.
                            swEncrypt.Write(source);

                        }

                        csEncrypt.FlushFinalBlock();
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }


            // Return the encrypted bytes from the memory stream.          
            return Encoding.Unicode.GetString(encrypted);
        }

        public byte[] Encrypt(byte[] bytesToBeEncrypted, byte[] passwordBytes)
        {
            byte[] encryptedBytes = null;

            // Set your salt here, change it to meet your flavor:
            // The salt bytes must be at least 8 bytes.
            byte[] saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };

            using (MemoryStream ms = new MemoryStream())
            {
                using (AesManaged AES = new AesManaged())
                {

                    var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
                    AES.Key = key.GetBytes(AES.KeySize / 8);
                    AES.IV = key.GetBytes(AES.BlockSize / 8);

                    using (var cs = new CryptoStream(ms, AES.CreateEncryptor(AES.Key, AES.IV), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
                        cs.FlushFinalBlock();
                    }
                }
                encryptedBytes = ms.ToArray();
            }

            return encryptedBytes;
        }

        public byte[] Decrypt(byte[] bytesToBeDecrypted, byte[] passwordBytes)
        {
            byte[] decryptedBytes = null;

            // Set your salt here, change it to meet your flavor:
            // The salt bytes must be at least 8 bytes.
            byte[] saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };

            using (MemoryStream ms = new MemoryStream(bytesToBeDecrypted))
            {
                using (AesManaged AES = new AesManaged())
                {
                    var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
                    AES.Key = key.GetBytes(AES.KeySize / 8);
                    AES.IV = key.GetBytes(AES.BlockSize / 8);
                    using (MemoryStream outputStream = new MemoryStream())
                    {
                        using (var cs = new CryptoStream(ms, AES.CreateDecryptor(AES.Key, AES.IV), CryptoStreamMode.Read))
                        {
                            //cs.Write(bytesToBeDecrypted, 0, bytesToBeDecrypted.Length);                    

                            byte[] buffer = new byte[1024];
                            for (int bytesReead = cs.Read(buffer, 0, buffer.Length); bytesReead > 0; bytesReead = cs.Read(buffer, 0, buffer.Length))
                            {
                                outputStream.Write(buffer, 0, bytesReead);
                            }

                        }
                        decryptedBytes = outputStream.ToArray();
                    }
                    //decryptedBytes = ms.ToArray();
                }
            }

            return decryptedBytes;
        }

        public string DecryptString(string stringToBeDecrypted, string password)
        {
            return Encoding.UTF8.GetString(Decrypt(Convert.FromBase64String(stringToBeDecrypted), Encoding.UTF8.GetBytes(password)));
        }

        public string EncryptString(string stringToBeEncrypted, string password)
        {
            return Convert.ToBase64String(
               Encrypt(Encoding.UTF8.GetBytes(stringToBeEncrypted), Encoding.UTF8.GetBytes(password)));
        }
    }


}
