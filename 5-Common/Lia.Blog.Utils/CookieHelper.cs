using System;
using System.Text;
using System.Web;
using System.Security.Cryptography;
using System.IO;
using System.Net.Http;

namespace Lia.Blog.Utils
{
    public class CookieHelper
    {
        /// <summary>
        /// 保存sessionId的cookies key值
        /// </summary>
        public static readonly string SessionIdKey = "sid";

        /// <summary>
        /// 保存sessionId的cookies Domain值
        /// </summary>
        public static readonly string CookiesDomain = ".liablog.com";

        public static readonly string RememberMe = "Blog_RememberMe";
       

        /// <summary>
        /// 清除当前会话中的一些cookie
        /// </summary>
        /// <param name="cookies"></param>
        /// <remarks>将会话置为过期（将Cookie的过期时间向前推一年）。</remarks>
        public static void Expires(string[] cookies)
        {
            try
            {
                var expires = DateTime.UtcNow.AddYears(-1);
                foreach (var cookie in cookies)
                {
                    HttpContext.Current.Response.Cookies.Add(new System.Web.HttpCookie(cookie)
                    {
                        Value = "",
                        Domain = CookiesDomain,
                        Path = "/",
                        Expires = expires
                    });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void SetCookie(string key, string value, DateTime? expireDate)
        {
            HttpCookie cookie = new HttpCookie(key);
            cookie.Expires = expireDate == null ? DateTime.Now.AddMinutes(1) : expireDate.Value;//DateTime.Now.AddMonths(3);
            cookie.Value = value;
            cookie.Domain = CookiesDomain;
            cookie.Path = "/";
            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        /// <summary>
        /// 获取Cookie。注：区分大小写
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static HttpCookie GetCookie(string name)
        {
            if (string.IsNullOrEmpty(name))
                return null;

            // 如果任意cookie中包含非法的字符（如：<、>等）,取cookie时会受到影响。
            // 第一次取抛出异常后，再取时就不会有问题了（不知道这个是不是.net自身的bug）。
            try
            {
                HttpCookieCollection cookies = HttpContext.Current.Request.Cookies;
                return cookies[name];
            }
            catch (Exception ex)
            {
                return null;
                throw ex;
            }
        }

        public static string EncryptCookies(String inputText)
        {
            return Encrypt(inputText, "&%#@?,:*");
        }
        /// <summary>
        /// 对Cookies进行解密的方法
        /// </summary>
        /// <param name="inputText"></param>
        /// <returns></returns>
        public static string DecryptCookies(String inputText)
        {
            return string.IsNullOrEmpty(inputText) ? "" : Decrypt(inputText, "&%#@?,:*");

        }

        /// <summary>
        /// 加密函数
        /// </summary>
        /// <param name="inputText"></param>
        /// <param name="strEncrKey"></param>
        /// <returns></returns>
        private static String Encrypt(String inputText, String strEncrKey)
        {
            byte[] byKey = { };
            byte[] iv = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
            try
            {
                byKey = Encoding.UTF8.GetBytes(strEncrKey.Substring(0, 8));
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                Byte[] inputByteArray = Encoding.UTF8.GetBytes(inputText);
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(byKey, iv), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                return Convert.ToBase64String(ms.ToArray());

            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        /// <summary>
        /// 解密函数
        /// </summary>
        /// <param name="inputText"></param>
        /// <param name="sDecrKey"></param>
        /// <returns></returns>
        private static string Decrypt(string inputText, string sDecrKey)
        {
            Byte[] byKey = { };
            Byte[] IV = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
            Byte[] inputByteArray = new byte[inputText.Length];
            try
            {
                byKey = Encoding.UTF8.GetBytes(sDecrKey.Substring(0, 8));
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                inputByteArray = Convert.FromBase64String(inputText);
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(byKey, IV), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                Encoding encoding = Encoding.UTF8;
                return encoding.GetString(ms.ToArray());
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
