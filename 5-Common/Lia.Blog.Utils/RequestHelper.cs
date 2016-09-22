using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace Lia.Blog.Utils
{
    public static class RequestHelper
    {
        public static string MapPath(string path)
        {
            if (HttpContext.Current != null)
            {
                return HttpContext.Current.Server.MapPath(path);
            }
            path = Regex.Replace(path, "\\A~?/", "").Replace("/", "\\");
            return Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, path);
        }
        public static string Query(string key)
        {
            string text = HttpContext.Current.Request.QueryString[key];
            if (!string.IsNullOrEmpty(text))
            {
                return HttpUtility.UrlDecode(RequestHelper.Safe(text.Trim()));
            }
            return string.Empty;
        }
        public static string RawUrl()
        {
            string text = HttpContext.Current.Request.ServerVariables["HTTP_X_REWRITE_URL"];
            if (string.IsNullOrEmpty(text))
            {
                return HttpContext.Current.Request.RawUrl;
            }
            return string.Format("http://{0}{1}", HttpContext.Current.Request.Url.Host, text);
        }
        public static string UrlParams()
        {
            string text = RequestHelper.RawUrl();
            return text.Substring(text.IndexOf('?') + 1);
        }
        public static string Params(string key)
        {
            string text = HttpContext.Current.Request.Params[key];
            return text ?? string.Empty;
        }
        public static string Form(string key)
        {
            string text = HttpContext.Current.Request.Form[key];
            if (text != null)
            {
                return text.Trim();
            }
            return string.Empty;
        }
        public static string Safe(string str)
        {
            return str.Replace("'", string.Empty);
        }
        public static string NonHtml(string str)
        {
            return Regex.Replace(str, "</?[^<]+>", "");
        }
        public static string GetIp(HttpContext context = null)
        {
            context = (context ?? HttpContext.Current);
            if (context == null)
            {
                return "127.0.0.1";
            }
            string text = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] ?? context.Request.Headers["X-Forwarded-For"];
            string text2 = context.Request.ServerVariables["REMOTE_ADDR"];
            string text3 = context.Request.ServerVariables["HTTP_VIA"];
            string[] array = (text ?? (string.IsNullOrEmpty(text3) ? text2 : text3)).Split(new char[]
            {
                ','
            });
            if (array.Length <= 1)
            {
                return array[0];
            }
            return array[array.Length - 1];
        }
        public static int GetInt(string str, int defaultValue = 0, HttpContext context = null)
        {
            context = (context ?? HttpContext.Current);
            int result;
            if (!int.TryParse(context.Request[str], out result))
            {
                result = defaultValue;
            }
            return result;
        }
        public static DateTime GetDate(string str, DateTime defatulValue)
        {
            DateTime result;
            if (!DateTime.TryParse(HttpContext.Current.Request[str], out result))
            {
                result = defatulValue;
            }
            return result;
        }
        public static void Rediret301(string url)
        {
            RequestHelper.Rediret301(HttpContext.Current, url);
        }
        public static void Rediret301(HttpContext context, string url)
        {
            context.Response.Clear();
            context.Response.Status = "301 Moved Permanently";
            context.Response.AddHeader("Location", url);
            context.Response.End();
        }
    }
}
