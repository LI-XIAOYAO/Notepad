using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;

namespace Notepad.Extensions
{
    /// <summary>
    /// EncryptExtension
    /// </summary>
    internal static class EncryptExtension
    {
        /// <summary>
        /// Encrypt
        /// </summary>
        /// <param name="value"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string Encrypt(this string value, string key)
        {
            try
            {
                using var aes = Aes.Create();
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = aes.Key.Take(16).ToArray();
                var content = Encoding.UTF8.GetBytes(value);

                using var crypto = aes.CreateEncryptor();

                return Convert.ToBase64String(crypto.TransformFinalBlock(content, 0, content.Length));
            }
            catch
            {
                throw new Exception("Encrypt failed");
            }
        }

        /// <summary>
        /// Encrypt
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string Encrypt<T>(this T value, string key)
            where T : class, new()
        {
            return JsonConvert.SerializeObject(value ?? new T()).Encrypt(key);
        }

        /// <summary>
        /// Decrypt
        /// </summary>
        /// <param name="value"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string Decrypt(this string value, string key)
        {
            try
            {
                using var aes = Aes.Create();
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = aes.Key.Take(16).ToArray();
                var content = Convert.FromBase64String(value);

                using var crypto = aes.CreateDecryptor();

                return Encoding.UTF8.GetString(crypto.TransformFinalBlock(content, 0, content.Length));
            }
            catch
            {
                throw new Exception("Decrypt failed");
            }
        }

        /// <summary>
        /// Decrypt
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T Decrypt<T>(this string value, string key)
            where T : class, new()
        {
            return JsonConvert.DeserializeObject<T>(value.Decrypt(key))!;
        }

        /// <summary>
        /// Md5
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string Md5(this string value)
        {
            return string.Concat(MD5.HashData(Encoding.UTF8.GetBytes(value)).Select(c => c.ToString("X2")));
        }
    }
}