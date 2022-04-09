using System.Security.Cryptography;
using System.Text;

namespace ProjectManager.Services
{
    public class CryptoService : ICryptoService
    {
        byte[] crypto_key;
        byte[] salt;
        public CryptoService(IConfiguration config)
        {
            crypto_key = Convert.FromBase64String(config["AppSettings:secret"]);
            salt = Convert.FromBase64String(config["AppSettings:salt"]);
        }
        public string Encrypt(string data)
        {
            using (var aes = Aes.Create())
            using (var encryptor = aes.CreateEncryptor(crypto_key, salt))
            {
                var plainText = Encoding.UTF8.GetBytes(data);
                byte[] model = encryptor.TransformFinalBlock(plainText, 0, plainText.Length);
                return Convert.ToBase64String(model);
            }
        }

        public string Decrypt(string data)
        {
            var buffer = Convert.FromBase64String(data);

            using (var aes = Aes.Create())
            using (var encryptor = aes.CreateDecryptor(crypto_key, salt))
            {
                var decryptedBytes = encryptor
                    .TransformFinalBlock(buffer, 0, buffer.Length);
                return Encoding.UTF8.GetString(decryptedBytes);
            }
        }
    }
}
