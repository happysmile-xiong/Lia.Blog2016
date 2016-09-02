namespace Lia.Blog.Infrastructure.Migrations
{
    using Domain.Entity;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<BlogDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(BlogDbContext context)
        {
            PerformInitialSetup(context);
            base.Seed(context);
        }

        public void PerformInitialSetup(BlogDbContext context)
        {
            AppUserManager userMgr = new AppUserManager(new UserStore<User>(context));
            AppRoleManager roleMgr = new AppRoleManager(new RoleStore<Domain.Dto.RoleDto>(context));

            string roleName = "Administrators";
            string userName = "Admin";
            string password = "Admin123";
            string email = "admin@balala.com";

            if (!roleMgr.RoleExists(roleName))
                roleMgr.Create(new Domain.Dto.RoleDto() { Name = roleName });

            User user = userMgr.FindByName(userName);
            if (user == null)
            {
                userMgr.Create(new User() { UserName = userName, Email = email, CreationTime = DateTime.Now }, password);
                user = userMgr.FindByName(userName);
            }
            if (!userMgr.IsInRole(user.Id, roleName))
                userMgr.AddToRole(user.Id, roleName);

            //foreach (User mUser in userMgr.Users)
            //    mUser.City = City.Paris;
            //context.SaveChanges();

            foreach(User mUser in userMgr.Users)
            {
                if (mUser.Country == Country.None)
                    mUser.SetCountryByCity(mUser.City);
            }
        }
    }
}
