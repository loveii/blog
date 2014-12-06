using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace Loveii.Helpers
{
    public class SecurityHelper
    {
        /// <summary>
        /// md5加密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string MD5(string str)
        {
            return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(str, "md5").ToLower();
        }

        /// <summary>
        /// SHA1加密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string SHA1(string str)
        {
            return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(str, "SHA1").ToLower();
        }

        /// <summary>
        /// Base64加密
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string EncodeBase64(string value)
        {
            if (string.IsNullOrEmpty(value))
                return "";
            
            return Convert.ToBase64String(System.Text.ASCIIEncoding.UTF8.GetBytes(value));
        }

        /// <summary>
        /// Base64解密
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string DecodeBase64(string value)
        {
            if (string.IsNullOrEmpty(value))
                return "";

            try
            {
                return System.Text.ASCIIEncoding.UTF8.GetString(Convert.FromBase64String(value));
            }
            catch
            {
                return "";
            }
        }
    }
}
