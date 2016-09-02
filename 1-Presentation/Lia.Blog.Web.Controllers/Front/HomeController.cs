using Lia.Blog.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Lia.Blog.Web.Controllers.Front
{
    public class HomeController : BaseController
    {
        [Authorize]
        public ActionResult Index()
        {
            return View(GetData("Index"));
        }

        [Authorize(Roles = "Users")]
        public ActionResult OtherAction()
        {
            return View(GetData("OtherAction"));
        }

        private Dictionary<string,object> GetData(string actionName)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            dict.Add("Action", actionName);
            var loginInfo = HttpContext.User.Identity;
            dict.Add("User", loginInfo.Name);
            dict.Add("Authenticated", loginInfo.IsAuthenticated);
            dict.Add("Auth Type", loginInfo.AuthenticationType);
            dict.Add("In Users Role", HttpContext.User.IsInRole("Users"));
            return dict;
        }

        [Authorize]
        public ActionResult UserProps()
        {
            return View(CurrentUser);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> UserProps(City city)
        {
            CurrentUser.City = city;
            CurrentUser.SetCountryByCity(city);
            await UserManager.UpdateAsync(CurrentUser);
            return View(CurrentUser);
        }
    }
}
