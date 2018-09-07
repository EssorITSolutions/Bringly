using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;

namespace Utilities.Helper
{
    public abstract class UtilityHelper
    {
        public static void GetIpValue(out string ip)
        {
            ip = string.Empty;
            ip = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(ip))
            {
                ip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }
        }

        public static void GetHostedDomainName(out string domainName)
        {
            domainName = $"{HttpContext.Current.Request.Url.Scheme}{System.Uri.SchemeDelimiter}{HttpContext.Current.Request.Url.Authority}";
        }

        public static string GetEncryptedPasswordHashedCode(string password, byte[] salt)
        {
            string passwordHashedCode = string.Empty;
            var pBKDF2 = new Rfc2898DeriveBytes(password, salt, 10000);

            byte[] hash = pBKDF2.GetBytes(20);
            byte[] hashedBytes = new byte[36];
            Array.Copy(salt, 0, hashedBytes, 0, 16);
            Array.Copy(hash, 0, hashedBytes, 16, 20);
            passwordHashedCode = Convert.ToBase64String(hashedBytes);
            return passwordHashedCode;
        }

        public static byte[] GetSalt(int size = 16)
        {
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[size]);
            return salt;
        }

        public static bool ValidatePassword(string password, byte[] salt, string hashedPassword)
        {
            if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(hashedPassword) || salt == null)
                return false;

            var pBKDF2 = new Rfc2898DeriveBytes(password, salt, 10000);
            byte[] hash = pBKDF2.GetBytes(20);
            byte[] hashedBytes = new byte[36];
            Array.Copy(salt, 0, hashedBytes, 0, 16);
            Array.Copy(hash, 0, hashedBytes, 16, 20);
            string passwordHashed = Convert.ToBase64String(hashedBytes);
            return hashedPassword.Equals(passwordHashed);
        }
    }

    public static class ExtentionMethodHelper
    {

        public static string GetHostedDomainName(this HtmlHelper htmlHelper)
        {
            return $"{HttpContext.Current.Request.Url.Scheme}{System.Uri.SchemeDelimiter}{HttpContext.Current.Request.Url.Authority}";
        }

        public static string GetDisplayName(this Enum val)
        {
            return val.GetType()
                      .GetMember(val.ToString())
                      .FirstOrDefault()
                      ?.GetCustomAttribute<DisplayAttribute>(false)
                      ?.Name
                      ?? val.ToString();
        }
    }
}
