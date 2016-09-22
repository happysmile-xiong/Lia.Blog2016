using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace Lia.Blog.Utils
{
    public static class ExtensionHelper
    {
        public static T Convert<T>(this string str, T defaultValue)
        {
            if (string.IsNullOrEmpty(str))
            {
                return defaultValue;
            }
            Type type = typeof(T);
            string fullName = type.FullName;
            if (type.Name == "Nullable`1")
            {
                Match match = Regex.Match(fullName, "((?<=\\[)(\\w+.\\w+)(?=,))");
                type = Type.GetType(match.Value);
            }
            MethodInfo methodInfo = null;
            MethodInfo[] methods = type.GetMethods();
            for (int i = 0; i < methods.Length; i++)
            {
                MethodInfo methodInfo2 = methods[i];
                if (methodInfo2.Name == "Parse" && methodInfo2.GetParameters().Length == 1)
                {
                    methodInfo = methodInfo2;
                    break;
                }
            }
            T result = defaultValue;
            try
            {
                if (methodInfo != null)
                {
                    result = (T)((object)methodInfo.Invoke(null, new string[]
                    {
                        str
                    }));
                }
            }
            catch
            {
                return defaultValue;
            }
            return result;
        }
        public static Guid ToGuid(this string str)
        {
            Guid result;
            try
            {
                result = new Guid(str);
            }
            catch
            {
                result = Guid.Empty;
            }
            return result;
        }
        public static string ToNumber(this string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return "";
            }
            return Regex.Replace(str, "\\D+", "");
        }
        public static int ToInt(this string str, int defaultValue)
        {
            int result;
            try
            {
                result = int.Parse(str);
            }
            catch
            {
                result = defaultValue;
            }
            return result;
        }
        public static int ToInt(this object str, int defaultValue)
        {
            int result;
            try
            {
                result = ConvertHelper.GetInteger(str);
            }
            catch
            {
                result = defaultValue;
            }
            return result;
        }
        public static int ToInt(this string str)
        {
            int result;
            try
            {
                result = int.Parse(str);
            }
            catch
            {
                result = -1;
            }
            return result;
        }
        public static int? ToIntOrNull(this string str)
        {
            if (str == string.Empty)
            {
                return null;
            }
            return new int?(str.ToInt());
        }
        public static short ToShortInt(this object obj)
        {
            return ConvertHelper.GetShortInt(ConvertHelper.GetString(obj));
        }
        public static long ToLong(this object obj)
        {
            return ConvertHelper.GetLong(ConvertHelper.GetString(obj));
        }
        public static double ToDouble(this object obj)
        {
            return ConvertHelper.GetDouble(ConvertHelper.GetString(obj));
        }
        public static float ToFloat(this string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return -1f;
            }
            str = Regex.Replace(str, "[^0-9^-^.]", string.Empty);
            if (str != "")
            {
                str = str.GetLeft(1) + str.Substring(1).Replace('-', '\0');
            }
            int num = str.IndexOf('.');
            if (num == -1)
            {
                return float.Parse(str);
            }
            str = str.Substring(0, num) + "." + str.Substring(num + 1).Replace('.', '\0');
            int num2;
            int.TryParse(str, out num2);
            return (float)num2;
        }
        public static bool ToBoolean(this object obj)
        {
            return obj != DBNull.Value && obj != null && ConvertHelper.GetString(obj).ToLower() == "true";
        }
        public static decimal ToDecimal(this object obj)
        {
            return ConvertHelper.GetDecimal(ConvertHelper.GetString(obj));
        }
        public static string ToDateTimeString(this object obj, string sFormat)
        {
            return ConvertHelper.GetDateTime(obj).ToString(sFormat);
        }
        public static string ToShortDateString(this object obj)
        {
            return ConvertHelper.GetDateTimeString(obj, "yyyy-MM-dd");
        }
        public static DateTime ToDateTime(object obj)
        {
            DateTime result;
            DateTime.TryParse(ConvertHelper.GetString(obj), out result);
            return result;
        }
        public static string GetLeft(this string str, int length)
        {
            if (string.IsNullOrEmpty(str))
            {
                return "";
            }
            if (length < str.Length)
            {
                return str.Substring(0, length);
            }
            return str;
        }
        public static string Substring2(this string str, int length)
        {
            if (string.IsNullOrEmpty(str))
            {
                return "";
            }
            if (length < str.Length)
            {
                return str.Substring(0, length);
            }
            return str;
        }
        public static string GetRight(this string str, int length)
        {
            if (string.IsNullOrEmpty(str))
            {
                return "";
            }
            if (length < str.Length)
            {
                return str.Substring(str.Length - length);
            }
            return str;
        }
        public static string UrlEncode(this string str)
        {
            return HttpUtility.UrlEncode(str);
        }
        public static string UrlDecode(this string str)
        {
            return HttpUtility.UrlDecode(str);
        }
        public static int[] ToIntArray(this string str, string separator)
        {
            if (str == null)
            {
                return new int[0];
            }
            string[] array = string.IsNullOrEmpty(separator) ? Regex.Split(str, "(?!^|$)") : str.Split(new string[]
            {
                separator
            }, StringSplitOptions.RemoveEmptyEntries);
            int[] array2 = new int[array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                array2[i] = array[i].ToInt(0);
            }
            return array2;
        }
    }
}
