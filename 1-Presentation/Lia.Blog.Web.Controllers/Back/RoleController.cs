using Lia.Blog.Domain.Dto;
using Lia.Blog.Domain.Entity;
using Lia.Blog.Domain.Model;
using Lia.Blog.Infrastructure;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Lia.Blog.Web.Controllers.Back
{
    [Authorize(Roles = "Administrators")]
    public class RoleController : BaseController
    {
        public ActionResult Index()
        {
            return View(RoleManager.Roles);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create([Required] string name)
        {
            if(ModelState.IsValid)
            {
                IdentityResult result = await RoleManager.CreateAsync(new RoleDto(name));
                if (result.Succeeded)
                    return RedirectToAction("Index", "Role", new { area = "Back" });
                else
                    AddErrorsFromResult(result);
            }
            return View(name);
        }

        public async Task<ActionResult> Edit(string id)
        {
            if(string.IsNullOrEmpty(id))
                return Error(new string[] { "Role Not Found" });
            
            RoleDto role = await RoleManager.FindByIdAsync(id);
            if(role==null)
                return Error(new string[] { "Role Not Found" });

            string[] memberIds = role.Users.Select(u => u.UserId).ToArray();
            IEnumerable<User> members = UserManager.Users.Where(u => memberIds.Any(a => a == u.Id));
            IEnumerable<User> nonMembers = UserManager.Users.Except(members);
            return View(new RoleEditModel() { Role = role, Members = members, NonMembers = nonMembers });
        }

        [HttpPost]
        public async Task<ActionResult> Edit(RoleModificationModel model)
        {
            IdentityResult result;
            if(ModelState.IsValid)
            {
                foreach(string userId in model.IdsToAdd??new string[] { })
                {
                    result = await UserManager.AddToRoleAsync(userId, model.RoleName);
                    if (!result.Succeeded)
                        return Error(result.Errors.ToArray());
                }

                foreach(string userId in model.IdsToDelete ?? new string[] { })
                {
                    result = await UserManager.RemoveFromRoleAsync(userId, model.RoleName);
                    if (!result.Succeeded)
                        return Error(result.Errors.ToArray());
                }
                return RedirectToAction("Index", "Role", new { area = "Back" });
            }
            return Error(new string[] { "Role Not Found" });
        }

        [HttpPost]
        public async Task<ActionResult> Delete(string id)
        {
            var role = await RoleManager.FindByIdAsync(id);
            if (role != null)
            {
                IdentityResult result = await RoleManager.DeleteAsync(role);
                if (result.Succeeded)
                    return RedirectToAction("Index", "Role", new { area = "Back" });
                else
                    return Error(result.Errors.ToArray());
            }
            else
            {
                return Error(new string[] { "Role Not Found" });
            }
        }
        
    }
}
