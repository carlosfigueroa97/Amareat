using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Amareat.Helpers;
using Amareat.Services.Encryption.Interfaces;

namespace Amareat.Services.Encryption.Implementations
{
    public class EncryptionService : IEncryptionService
    {
        public EncryptionService()
        {
        }

        #region Private Methods

        private byte [] CreateKey(string value)
        {
            byte[] salt = new byte[] { 80, 70, 60, 50, 40, 30, 20, 10 };
            var keyGenerator = new Rfc2898DeriveBytes(value, salt, 300);
            return keyGenerator.GetBytes(32);
        }

        private byte[] AesEncryptStringToBytes(string plainText, byte[] key, byte [] iv)
        {
            if(plainText?.Length <= 0 || key?.Length <= 0 || iv.Length <= 0)
            {
                throw new Exception();
            }

            byte[] encrypted;

            Aes aes = Aes.Create();

            aes.Key = key;
            aes.IV = iv;

            MemoryStream memoryStream = new MemoryStream();

            var encryptor = aes.CreateEncryptor();
            var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
            var streamWriter = new StreamWriter(cryptoStream);
            streamWriter.Write(plainText);

            encrypted = memoryStream.ToArray();

            return encrypted;
        }

        #endregion

        #region Public Methods

        public string Decrypt(string value)
        {
            throw new NotImplementedException();
        }

        public string Encrypt(string value)
        {
            Aes aes = Aes.Create();

            aes.Key = Encoding.ASCII.GetBytes(ConstantGlobal.SecretKey);
            aes.IV = Encoding.ASCII.GetBytes(ConstantGlobal.IV);
            aes.Mode = CipherMode.CBC;

            var encryptor = aes.CreateEncryptor();

            var memoryStream = new MemoryStream();

            var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);

            cryptoStream.Write(Encoding.UTF8.GetBytes(value), 0, value.Length);

            cryptoStream.FlushFinalBlock();
            var result = BitConverter.ToString(memoryStream.ToArray())
                .Replace("-", string.Empty);

            return result;
        }

        #endregion
    }
}
