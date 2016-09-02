using Lia.Blog.Application.Interfaces;
using Lia.Blog.Domain.Dto;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Lia.Blog.Domain.Entity;
using Microsoft.AspNet.Identity;
using System.Security.Claims;
using Microsoft.Owin.Security;
using Lia.Blog.Infrastructure;

namespace Lia.Blog.Web.Controllers.Front
{
    [Authorize]
    public class AccountController : BaseController
    {
        private readonly IUserService _userService;
        public AccountController()
        {
            _userService = ServiceLocator.Current.GetInstance<IUserService>();
        }

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            if(HttpContext.User.Identity.IsAuthenticated)
            {
                return Error(new string[] { "Access Denied" });
            }
            ViewBag.returnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginDto mLogin, string returnUrl)
        {
            //var result = _userService.Login(mLogin);
            //ViewBag.SubmitResult = (string.Format("CallBack({0},\"{1}\")", (int)result, result.ToString()));

            if (ModelState.IsValid)
            {
                User user = await UserManager.FindAsync(mLogin.UserName, mLogin.Password);
                if (user == null)
                {
                    ModelState.AddModelError("", "Invalid name or password");
                }
                else
                {
                    ClaimsIdentity ident = await UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
                    ident.AddClaims(LocationClaimsProvider.GetClaims(ident));
                    ident.AddClaims(ClaimsRoles.CreateRolesFromClaims(ident));
                    AuthManager.SignOut();
                    AuthManager.SignIn(new AuthenticationProperties() { IsPersistent = false }, ident);
                    if (string.IsNullOrEmpty(returnUrl))
                        return RedirectToAction("Index", "Home", new { area = "Front" });
                    else
                        return Redirect(returnUrl);
                }
            }

            ViewBag.returnUrl = returnUrl;
            return View(mLogin);
        }

        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Register(UserViewDto userView)
        {
            //var result = _userService.Register(mLogin, Request.Form["confirmPass"]);
            //ViewBag.SubmitResult = (string.Format("CallBack({0},\"{1}\")", (int)result, result.ToString()));

            if (ModelState.IsValid)
            {
                var nickName = string.IsNullOrEmpty(userView.NickName) ? userView.UserName : userView.NickName;
                User user = new User() { UserName = userView.UserName, NickName = nickName.Length>50? nickName.Substring(0,50):nickName,
                    Email = userView.Email, CreationTime = DateTime.Now, LastModifiedTime = DateTime.Now };
                IdentityResult result = await UserManager.CreateAsync(user, userView.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction("Login", "Account", new { area = "Front" });
                }
                else
                {
                    AddErrorsFromResult(result);
                }
            }
            return View(userView);
        }

        [Authorize]
        public ActionResult Logout()
        {
            AuthManager.SignOut();
            return RedirectToAction("Index", "Home", new { area = "Front" });
        }
    }
}
