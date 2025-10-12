using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace XBLMS.Utils
{
    public class AesEncryptor
    {
        public static (string aesKey, string aesIV, string aesSalt) GetKey()
        {
            var key = GenerateKey();
            var iv = GenerateIV();
            var salt = GetSalt(key, iv);
            return (key, iv, salt);
        }
        private static string GenerateKey()
        {
            return StringUtils.GetRandomString(32);
        }
        private static string GenerateIV()
        {
            return StringUtils.GetRandomString(16);
        }
        private static string GetSalt(string key, string iv)
        {
            return $"{StringUtils.GetRandomString(8)}{key}{StringUtils.GetRandomString(6)}{iv}{StringUtils.GetRandomString(8)}";
        }
        /// <summary>
        /// AES加密(CBC模式)
        /// </summary>
        public static string Encrypt(string plainText, string key, string iv)
        {
            using Aes aes = Aes.Create();
            aes.Key = Encoding.UTF8.GetBytes(key);
            aes.IV = Encoding.UTF8.GetBytes(iv);
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            using MemoryStream ms = new();
            using CryptoStream cs = new(ms, encryptor, CryptoStreamMode.Write);
            using (StreamWriter sw = new(cs))
            {
                sw.Write(plainText);
            }
            return Convert.ToBase64String(ms.ToArray());
        }

        /// <summary>
        /// AES解密(CBC模式)
        /// </summary>
        public static string Decrypt(string cipherText, string salt)
        {
            var (key, iv) = DecryptGetKey(salt);

            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using Aes aes = Aes.Create();
            aes.Key = Encoding.UTF8.GetBytes(key);
            aes.IV = Encoding.UTF8.GetBytes(iv);
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            using ICryptoTransform decryptor = aes.CreateDecryptor();
            byte[] decryptedBytes = decryptor.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);
            return Encoding.UTF8.GetString(decryptedBytes);
        }
        private static (string aesKey, string aesIV) DecryptGetKey(string salt)
        {
            var key = salt.Substring(8, 32);
            var iv = salt.Substring(46, 16);
            return (key, iv);
        }
    }
}

