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

        protected ActionResult Error(string error)
        {
            ModelState.AddModelError("", error);
            return View();
        }

        protected ActionResult Javascript(string alertMsg = "", string goUrl = "")
        {
            StringBuilder result = new StringBuilder();
            if (string.IsNullOrEmpty(alertMsg) && string.IsNullOrEmpty(goUrl))
                return View();

            result.Append("<script>");
            if (!string.IsNullOrEmpty(alertMsg))
                result.AppendFormat("alert('{0}');", alertMsg);
            if (!string.IsNullOrEmpty(goUrl))
                result.AppendFormat("location.href='{0}';", goUrl);

            result.Append("</script>");

            return Content(result.ToString());
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
