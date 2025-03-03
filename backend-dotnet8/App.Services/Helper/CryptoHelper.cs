using System.Security.Cryptography;
using System.Text;

namespace App.Services.Helper
{
    public class CryptoHelper
    {
        private static readonly string SecretKey = "jsil-hhhfd4hg302bvc688";

        private static byte[] GetKey()
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(SecretKey);
            Array.Resize(ref keyBytes, 32); // Ensure 32-byte key length
            return keyBytes;
        }

        public static string Encrypt(string plainText)
        {
            using (var aes = Aes.Create())
            {
                aes.Key = GetKey();
                aes.GenerateIV(); // Generate a random IV

                using (var ms = new MemoryStream())
                {
                    ms.Write(aes.IV, 0, aes.IV.Length); // Store IV at the start

                    using (var cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
                    using (var writer = new StreamWriter(cs, Encoding.UTF8))
                    {
                        writer.Write(plainText);
                    }

                    return Convert.ToBase64String(ms.ToArray());
                }
            }
        }

        public static string Decrypt(string? cipherText)
        {
            byte[] encryptedData = Convert.FromBase64String(cipherText);

            using (var aes = Aes.Create())
            {
                aes.Key = GetKey();
                byte[] iv = new byte[16];
                Array.Copy(encryptedData, iv, iv.Length); // Extract IV

                aes.IV = iv;

                using (var ms = new MemoryStream(encryptedData, iv.Length, encryptedData.Length - iv.Length))
                using (var cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Read))
                using (var reader = new StreamReader(cs, Encoding.UTF8))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}
