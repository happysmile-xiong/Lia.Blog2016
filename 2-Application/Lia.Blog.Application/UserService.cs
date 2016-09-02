using Lia.Blog.Application.Interfaces;
using Lia.Blog.Domain.Dto;
using Lia.Blog.Domain.Entity;
using Lia.Blog.Domain.IRepository;
using Lia.Blog.Infrastructure.Interfaces;
using Lia.Blog.Utils;
using Lia.Blog.Utils.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lia.Blog.Application
{
    public class UserService:IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;

        public UserService(IUnitOfWork unitOfWork, IUserRepository userRepository)
        {
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
        }

        public ResultCode Login(UserViewDto mLogin)
        {
            User mUser = null;
            var result = CheckLogin(mLogin, ref mUser);
            if (result != ResultCode.Success)
                return result;

            var loginInfo = new LoginInfo() { UserId = mUser.Id, UserName = mUser.UserName, NickName = mUser.NickName };
            AddLoginInfo(loginInfo);//添加用户信息到cookie

            //记住我
            if (mLogin.IsRemember)
            {
                CookieHelper.SetCookie(CookieHelper.RememberMe, mLogin.UserName, DateTime.Now.AddMonths(3));
            }
            else
            {
                var rememberMe = CookieHelper.GetCookie(CookieHelper.RememberMe);
                if (rememberMe == null)
                    CookieHelper.Expires(new[] { CookieHelper.RememberMe });
            }

            return result;
        }


        private ResultCode CheckLogin(UserViewDto mLogin, ref User mUser)
        {
            var result = ResultCode.Success;
            mUser = null;
            if ((!Util.IsUserName(mLogin.UserName)) || (!Util.IsPassword(mLogin.Password)))
                return ResultCode.UserPasswordError;

            var loginInfo = GetLoginInfo(new LoginInfo());
            if (loginInfo != null || (!string.IsNullOrEmpty(loginInfo.UserId)))//loginInfo.UserId > 0)
                return ResultCode.HasLogined;

            mUser = _userRepository.GetByName(mLogin.UserName);
            if (mUser == null)
                return ResultCode.UserPasswordError;

            if (!mUser.IsActive)
                return ResultCode.UnActive;

            var correctPass = PasswordSecurity.PasswordStorage.VerifyPassword(mLogin.Password, mUser.PasswordHash);
            if (!correctPass)
                return ResultCode.UserPasswordError;

            return result; ;
        }

        public ResultCode Register(UserViewDto mLogin, string confirmPass)
        {
            var result = CheckRegister(mLogin, confirmPass);
            if (result != ResultCode.Success)
                return result;
            var mUser = new User()
            {
                UserName = mLogin.UserName,
                NickName = string.IsNullOrEmpty(mLogin.NickName) ? mLogin.UserName : (mLogin.NickName.Length > 50 ? mLogin.NickName.Substring(0, 50) : mLogin.NickName),
                Email = mLogin.Email,
                EmailConfirmed = false,
                IsActive = false,
                PasswordHash = PasswordSecurity.PasswordStorage.CreateHash(mLogin.Password),
                CreationTime = DateTime.Now,
                LastModifiedTime = DateTime.Now
            };
            _unitOfWork.RegisterNew(mUser);
            _unitOfWork.CommitAsync();


            return result;
        }

        public ResultCode CheckRegister(UserViewDto mLogin, string confirmPass)
        {
            var result = ResultCode.Success;
            if (mLogin == null || string.IsNullOrEmpty(mLogin.UserName) || (!Util.IsUserName(mLogin.UserName)))
            {
                return ResultCode.UserNameError;
            }
            if (string.IsNullOrEmpty(mLogin.Password) || (string.IsNullOrEmpty(confirmPass)) || (!Util.IsPassword(mLogin.Password)) || (!mLogin.Password.Equals(confirmPass)))
            {
                return ResultCode.PasswordError;
            }

            var mUser = _userRepository.GetByName(mLogin.UserName);
            if (mUser != null)
            {
                return ResultCode.UserNameExists;
            }

            return result;
        }


        /// <summary>
        /// 添加用户登录信息到cookie
        /// </summary>
        /// <param name="loginInfo"></param>
        private void AddLoginInfo(LoginInfo loginInfo)
        {
            var userInfo = CookieHelper.EncryptCookies(JsonHelper.ObjectToJson(loginInfo));
            CookieHelper.SetCookie(loginInfo.Key, userInfo, DateTime.Now.AddYears(1));
        }

        /// <summary>
        /// 获取用户登录信息
        /// </summary>
        /// <param name="loginInfo"></param>
        public LoginInfo GetLoginInfo(LoginInfo loginInfo)
        {
            try
            {
                var cookie = CookieHelper.GetCookie(loginInfo.Key);
                var userInfo = (cookie != null && String.IsNullOrEmpty(cookie.Value) == false) ? cookie.Value : null;
                if (string.IsNullOrEmpty(userInfo))
                    return null;
                loginInfo = JsonHelper.JsonToObject<LoginInfo>(CookieHelper.DecryptCookies(userInfo));
                return loginInfo;
            }
            catch (Exception ex)
            {
                return new LoginInfo() { UserId = "", UserName = string.Empty, NickName = string.Empty };
                throw ex;
            }

        }
    }
}
