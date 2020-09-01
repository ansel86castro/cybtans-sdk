using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Cybtans.Services.Security
{
    public abstract class CryptoService : ICryptoService
    {
        public virtual byte[] ComputeHash(byte[] data)
        {
            byte[] result = null;
            using (SHA1 sha = new SHA1Managed()) //new SHA1CryptoServiceProvider();
            {
                result = sha.ComputeHash(data);
            }
            return result;
        }

        public virtual byte[] ComputeHash(Stream data)
        {
            byte[] result = null;
            using (SHA1 sha = new SHA1Managed()) //new SHA1CryptoServiceProvider();
            {
                result = sha.ComputeHash(data);
            }
            return result;
        }



        public virtual string ComputeHash(string input)
        {

            byte[] data = Encoding.UTF8.GetBytes(input);
            byte[] result = null;
            using (SHA1 sha = new SHA1Managed()) //new SHA1CryptoServiceProvider();
            {
                result = sha.ComputeHash(data);
            }

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                sb.Append(result[i].ToString("x2"));
            }
            var hash_string = sb.ToString();
            //var hash_string = Encoding.Unicode.GetString(result);
            return hash_string;
        }

        public static string ToStringX2(byte[] data)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sb.Append(data[i].ToString("x2"));
            }
            return sb.ToString();
        }

        public static byte[] FromStringX2(string data)
        {
            if (data.Length % 2 != 0)
                throw new InvalidOperationException($"Invalid format {nameof(data)}");

            var bytes = new byte[data.Length / 2];
            for (int i = 0; i < bytes.Length; i++)
            {
                byte b = Convert.ToByte(data[i * 2]);
                b = (byte)(b << 4 | Convert.ToByte(data[i * 2 + 1]));

                bytes[i] = b;
            }

            return bytes;
        }

        public abstract byte[] Encrypt(byte[] source);

        public abstract byte[] Decrypt(byte[] source);

        public abstract string Encrypt(string source);

        public abstract string Decrypt(string source);
    }

}
