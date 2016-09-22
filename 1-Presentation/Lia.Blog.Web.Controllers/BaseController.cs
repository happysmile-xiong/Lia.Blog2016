using Lia.Blog.Application.Interfaces;
using Lia.Blog.Domain;
using Lia.Blog.Domain.Dto;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web;
using Lia.Blog.Domain.Entity;
using Lia.Blog.Infrastructure;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace Lia.Blog.Web.Controllers
{
    public class BaseController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            CurrentUser = null;
            IsAdmin = false;
            var context = filterContext.HttpContext;
            if (context != null && context.User != null && context.User.Identity != null && context.User.Identity.IsAuthenticated)
            {
                CurrentUser = UserManager.FindByName(context.User.Identity.Name);
                IsAdmin = HttpContext.User.IsInRole("Administrators");
            }
        }

        public static User CurrentUser
        {
            get; set;
        }

        public static bool IsAdmin
        {
            get;set;
        }

        protected AppUserManager UserManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<AppUserManager>();
            }
        }

        protected AppRoleManager RoleManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<AppRoleManager>();
            }
        }

        protected IAuthenticationManager AuthManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        protected void AddErrorsFromResult(IdentityResult result)
        {
            foreach (string error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        public ActionResult Error(string[] strs)
        {
            return View("~/Views/Shared/Error.cshtml", strs);
        }
    }

}
