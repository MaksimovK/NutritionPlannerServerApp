using System.Security.Cryptography;
using System.Text;

namespace NutritionPlanner.Application.Services
{
    public class EncryptionService
    {
        private readonly byte[] _key;
        private const int IV_SIZE = 16; // 128 бит для AES

        public EncryptionService(byte[] key) => _key = key;

        public (string CipherText, string IV) Encrypt(string plainText)
        {
            using var aes = Aes.Create();
            aes.Key = _key;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            aes.GenerateIV(); // Генерируем корректный IV (16 байт)

            using var encryptor = aes.CreateEncryptor();
            byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
            byte[] cipherBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);

            return (
                Convert.ToBase64String(cipherBytes),
                Convert.ToBase64String(aes.IV) // IV всегда 16 байт -> 24 символа base64
            );
        }

        public string Decrypt(string cipherText, string iv)
        {
            // Проверка длины IV перед использованием
            byte[] ivBytes = Convert.FromBase64String(iv);
            if (ivBytes.Length != IV_SIZE)
            {
                throw new ArgumentException(
                    $"Недопустимая длина IV: {ivBytes.Length} байт. Требуется: {IV_SIZE} байт");
            }

            using var aes = Aes.Create();
            aes.Key = _key;
            aes.IV = ivBytes;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            using var decryptor = aes.CreateDecryptor();
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            byte[] plainBytes = decryptor.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);

            return Encoding.UTF8.GetString(plainBytes);
        }
    }
}
