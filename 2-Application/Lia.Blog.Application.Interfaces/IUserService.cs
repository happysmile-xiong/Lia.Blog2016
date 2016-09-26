using Lia.Blog.Domain.Dto;
using Lia.Blog.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lia.Blog.Application.Interfaces
{
    public interface IUserService
    {
        User GetUserById(string userId);

        ResultCode Login(UserViewDto mLogin);

        ResultCode Register(UserViewDto mLogin, string confirmPass);

        LoginInfo GetLoginInfo(LoginInfo loginInfo);
    }
}
