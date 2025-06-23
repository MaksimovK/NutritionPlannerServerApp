using System.Security.Cryptography;
using System.Text;

namespace NutritionPlanner.Application.Services
{
    public class EncryptionService
    {
        private readonly byte[] _key;

        public EncryptionService(byte[] key)
        {
            if (key.Length != 32)
                throw new ArgumentException("Key must be 256-bit (32 bytes)");

            _key = key;
        }

        public (string cipherText, string iv) Encrypt(string plainText)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = _key;
                aes.GenerateIV();
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
                byte[] cipherBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);

                return (
                    Convert.ToBase64String(cipherBytes),
                    Convert.ToBase64String(aes.IV)
                );
            }
        }

        public string Decrypt(string cipherText, string ivBase64)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = _key;
                aes.IV = Convert.FromBase64String(ivBase64);
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                byte[] cipherBytes = Convert.FromBase64String(cipherText);
                byte[] plainBytes = decryptor.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);

                return Encoding.UTF8.GetString(plainBytes);
            }
        }
    }
}