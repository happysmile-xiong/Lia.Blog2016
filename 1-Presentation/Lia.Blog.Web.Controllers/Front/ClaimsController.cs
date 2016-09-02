using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Lia.Blog.Web.Controllers.Front
{
    public class ClaimsController:BaseController
    {
        [Authorize]
        public ActionResult Index()
        {
            ClaimsIdentity ident = HttpContext.User.Identity as ClaimsIdentity;
            if (ident == null)
                return Error(new string[] { "No claims available." });
            return View(ident.Claims);
        }

        [Authorize(Roles = "BeijingStaff")]
        public string OtherAction()
        {
            return "Claim Role ddd";
        }
    }
}
