using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Lia.Blog.Utils
{
    public static class Util
    {
        /// <summary>
        /// 判断密码
        /// </summary>
        /// <param name="str">密码</param>
        /// <returns>bool</returns>
        public static bool IsPassword(string str)
        {
            if (str == null)
            {
                return false;
            }

            return Regex.IsMatch(str, @"^(?![A-Za-z]+$)(?!\d+$)[A-Za-z0-9]{6,20}$");
        }

        /// <summary>
        /// 获取字符串长度
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>int</returns>
        public static int StrLength(string str)
        {
            byte[] strArray = System.Text.Encoding.Default.GetBytes(str);
            return strArray.Length;
        }

        public static bool IsEmail(string str)
        {
            if (string.IsNullOrEmpty(str))
                return false;

            return Regex.IsMatch(str, @"^[A-Za-z0-9](([_\.\-]?[a-zA-Z0-9]+)*)@([A-Za-z0-9]+)(([\.\-]?[a-zA-Z0-9]+)*)\.([A-Za-z]{2,})$");
        }

        /// <summary>
        /// 判断用户名
        /// </summary>
        /// <param name="str">用户名</param>
        /// <returns>bool</returns>
        public static bool IsUserName(string str)
        {
            if (str == null)
            {
                return false;
            }

            if (StrLength(str) < 3 || StrLength(str) > 20)
            {
                return false;
            }

            return Regex.IsMatch(str, @"^[a-zA-Z0-9@_]{3,20}$");
        }
    }
}
