using System.Security.Cryptography;

namespace Service
{
    public static class CryptographyService
    {
        public static string Encrypt(string text)
        {
            var HashTool = SHA512.Create();
            byte[] PhraseAsByte = System.Text.Encoding.UTF8.GetBytes(string.Concat(text));
            byte[] EncryptedBytes = HashTool.ComputeHash(PhraseAsByte);
            HashTool.Clear();
            return Convert.ToBase64String(EncryptedBytes);
        }
    }
}
