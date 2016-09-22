using Lia.Blog.Application.Interfaces;
using Lia.Blog.Domain.Entity;
using Lia.Blog.Domain.Model;
using Lia.Blog.Utils;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Lia.Blog.Web.Controllers.Front
{
    [Authorize]
    public class HomeController : BaseController
    {
        private readonly IBlogService _blogService;
        public HomeController()
        {
            _blogService = ServiceLocator.Current.GetInstance<IBlogService>();
        }
        
        public ActionResult Index()
        {
            var parameter = new BlogParameter()
            {
                AuthorId = CurrentUser.Id,
                PageIndex = RequestHelper.Query("p").ToInt(0),
                PageSize = 20
            };
            var list = _blogService.GetBlogList(parameter).ToList();
            return View(list);
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
