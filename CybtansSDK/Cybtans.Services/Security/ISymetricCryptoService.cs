namespace Cybtans.Services.Security
{
    public interface ISymetricCryptoService
    {
        byte[] Decrypt(byte[] bytesToBeDecrypted, byte[] passwordBytes);

        byte[] Encrypt(byte[] bytesToBeEncrypted, byte[] passwordBytes);

        string DecryptString(string stringToBeDecrypted, string password);

        string EncryptString(string stringToBeEncrypted, string password);
    }
}