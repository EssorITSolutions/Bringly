using System;
using System.Configuration;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Utilities.Helper
{
    public static class EncryptionHelper
    {
        static EncryptionHelper()
        {

        }
        static readonly string PassPhrase = ConfigurationManager.AppSettings["passPhrase"];// can be any string
        private const string HashAlgorithm = "MD5"; // can be "MD5"
        static readonly int PasswordIterations = Convert.ToInt32(ConfigurationManager.AppSettings["passwordIterations"]);// can be any number
        static readonly string InitVector = ConfigurationManager.AppSettings["initVector"];// "~1B2c4D4d6F6F7H8"; // must be 16 bytes
        private const int KeySize = 256; // can be 192 or 128

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <param name="saltValue"></param>
        /// <returns></returns>
        public static string EncryptText(string text, string saltValue)
        {
            return Encrypt(text, saltValue, PassPhrase, HashAlgorithm, PasswordIterations, InitVector);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <param name="saltValue"></param>
        /// <returns></returns>
        public static string DecryptText(string text, string saltValue)
        {
            return Decryption(text, saltValue, PassPhrase, HashAlgorithm, PasswordIterations, InitVector);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <param name="saltValue"></param>
        /// <param name="passPhrase"></param>
        /// <param name="passwordIterations"></param>
        /// <param name="initVector"></param>
        /// <returns></returns>
        public static string EncryptText(string text, string saltValue, string passPhrase, int passwordIterations, string initVector)
        {
            return Encrypt(text, saltValue, passPhrase, HashAlgorithm, passwordIterations, initVector);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <param name="saltValue"></param>
        /// <param name="passPhrase"></param>

        /// <param name="passwordIterations"></param>
        /// <param name="initVector"></param>
        /// <returns></returns>
        public static string DecryptText(string text, string saltValue, string passPhrase, int passwordIterations, string initVector)
        {
            return Decryption(text, saltValue, passPhrase, HashAlgorithm, passwordIterations, initVector);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <param name="saltValue"></param>
        /// <param name="passPhrase"></param>
        /// <param name="hashAlgorithm"></param>
        /// <param name="passwordIterations"></param>
        /// <param name="initVector"></param>
        /// <returns></returns>
        private static string Decryption(string text, string saltValue, string passPhrase, string hashAlgorithm, int passwordIterations, string initVector)
        {
            try
            {
                saltValue = saltValue.ToLower();
                var bytes = Encoding.ASCII.GetBytes(initVector);
                var rgbSalt = Encoding.ASCII.GetBytes(saltValue);
                var buffer = Convert.FromBase64String(text);
                var rgbKey = new PasswordDeriveBytes(passPhrase, rgbSalt, hashAlgorithm, passwordIterations).GetBytes(KeySize / 8);
                var managed = new RijndaelManaged { Mode = CipherMode.CBC };
                var transform = managed.CreateDecryptor(rgbKey, bytes);
                var stream = new MemoryStream(buffer);
                var stream2 = new CryptoStream(stream, transform, CryptoStreamMode.Read);
                var buffer5 = new byte[buffer.Length];
                var count = stream2.Read(buffer5, 0, buffer5.Length);

                stream.Close();
                stream.Dispose();
                stream2.Close();
                stream2.Dispose();
                transform.Dispose();
                managed.Dispose();
                return Encoding.UTF8.GetString(buffer5, 0, count);
            }
            catch (Exception) { return text; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <param name="saltValue"></param>
        /// <param name="passPhrase"></param>
        /// <param name="hashAlgorithm"></param>
        /// <param name="passwordIterations"></param>
        /// <param name="initVector"></param>
        /// <returns></returns>
        private static string Encrypt(string text, string saltValue, string passPhrase, string hashAlgorithm, int passwordIterations, string initVector)
        {
            try
            {
                saltValue = saltValue.ToLower();
                var bytes = Encoding.ASCII.GetBytes(initVector);
                var rgbSalt = Encoding.ASCII.GetBytes(saltValue);
                var buffer = Encoding.UTF8.GetBytes(text);
                var rgbKey = new PasswordDeriveBytes(passPhrase, rgbSalt, hashAlgorithm, passwordIterations).GetBytes(KeySize / 8);
                var managed = new RijndaelManaged { Mode = CipherMode.CBC };
                var transform = managed.CreateEncryptor(rgbKey, bytes);
                var stream = new MemoryStream();
                var stream2 = new CryptoStream(stream, transform, CryptoStreamMode.Write);
                stream2.Write(buffer, 0, buffer.Length);
                stream2.FlushFinalBlock();
                var inArray = stream.ToArray();

                stream.Close();
                stream.Dispose();
                stream2.Close();
                stream2.Dispose();
                transform.Dispose();
                managed.Dispose();
                return Convert.ToBase64String(inArray);
            }
            catch { return text; }
        }
    }
}
