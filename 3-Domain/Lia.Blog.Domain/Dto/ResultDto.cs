using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lia.Blog.Domain.Dto
{
    public class ResultDto
    {
        public ResultCode ResultCode { get; set; }

        public string ResultMsg { get; set; }
    }

    public enum ResultCode
    {
        /// <summary>
        /// 成功
        /// </summary>
        Success = 1,

        UnActive = 2,

        /// <summary>
        /// 用户名或密码错误
        /// </summary>
        UserPasswordError = 3,

        /// <summary>
        /// 用户已登录
        /// </summary>
        HasLogined = 4,

        /// <summary>
        /// 用户名错误
        /// </summary>
        UserNameError = 5,

        /// <summary>
        /// 用户名已存在
        /// </summary>
        UserNameExists = 6,

        /// <summary>
        /// 密码错误
        /// </summary>
        PasswordError = 7,
    }
}
