using Lia.Blog.Domain.Dto;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lia.Blog.Infrastructure
{
    public class AppRoleManager : RoleManager<RoleDto>, IDisposable
    {
        public AppRoleManager(RoleStore<RoleDto> store) : base(store) { }

        public static AppRoleManager Create(
            IdentityFactoryOptions<AppRoleManager> options,IOwinContext context)
        {
            return new AppRoleManager(new RoleStore<RoleDto>(context.Get<BlogDbContext>()));
        }

    }
}
