using System.IO;

namespace Cybtans.Services.Security
{
    public interface ICryptoService
    {
        byte[] ComputeHash(Stream data);
        byte[] ComputeHash(byte[] data);
        string ComputeHash(string input);
        byte[] Decrypt(byte[] source);
        string Decrypt(string source);
        byte[] Encrypt(byte[] source);
        string Encrypt(string source);
    }
}