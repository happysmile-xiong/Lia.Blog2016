using Lia.Blog.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using System.Web;
using Lia.Blog.Domain.Entity;
using Microsoft.AspNet.Identity;
using Lia.Blog.Domain.Dto;

namespace Lia.Blog.Web.Controllers.Back
{
    [Authorize(Roles = "Administrators")]
    public class AdminController : BaseController
    {
        public ActionResult Index()
        {
            return View(UserManager.Users);
        }

        [HttpPost]
        public async Task<ActionResult> Delete(string id)
        {
            User user = await UserManager.FindByIdAsync(id);
            if (user != null)
            {
                IdentityResult result = await UserManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Admin", new { area = "Back" });
                }else {
                    return Error(result.Errors.ToArray());
                }
            }
            else
            {
                return Error(new string[] { "User Not Found" });
            }
        }

        public async Task<ActionResult> Edit(string id)
        {
            User user = await UserManager.FindByIdAsync(id);
            if (user != null)
            {
                var userView = new UserViewDto() { UserId = user.Id, UserName = user.UserName, NickName = user.NickName, Email = user.Email };
                return View(userView);
            }
            else
                return RedirectToAction("Index", "Admin", new { area = "Back" });
        }

        [HttpPost]
        public async Task<ActionResult> Edit(UserViewDto userView)
        {
            if (userView == null || string.IsNullOrEmpty(userView.UserId) || string.IsNullOrEmpty(userView.Email))
            {
                ModelState.AddModelError("", "Params Error.");
                return View(userView);
            }

            var user = await UserManager.FindByIdAsync(userView.UserId);
            if (user != null)
            {
                user.NickName = userView.NickName;
                user.Email = userView.Email;
                var nickName = string.IsNullOrEmpty(userView.NickName) ? userView.UserName : userView.NickName;
                user.NickName = nickName.Length > 50 ? nickName.Substring(0, 50) : nickName;
                IdentityResult validEmail = await UserManager.UserValidator.ValidateAsync(user);
                if (!validEmail.Succeeded)
                {
                    AddErrorsFromResult(validEmail);
                }
                IdentityResult validPass = (string.IsNullOrEmpty(userView.Password)) ? null : await UserManager.PasswordValidator.ValidateAsync(userView.Password);
                if (validPass != null && validPass.Succeeded)
                {
                    user.PasswordHash = UserManager.PasswordHasher.HashPassword(userView.Password);
                }
                else if (validPass != null && validPass.Errors.Any())
                {
                    AddErrorsFromResult(validPass);
                }

                if (validEmail.Succeeded && (validPass == null || validPass.Succeeded))
                {
                    IdentityResult result = await UserManager.UpdateAsync(user);
                    if (result.Succeeded)
                        return RedirectToAction("Index");
                    else
                        AddErrorsFromResult(result);
                }

            }
            else
                ModelState.AddModelError("", "User Not Found.");
            return View(userView);
        }
    }
}
