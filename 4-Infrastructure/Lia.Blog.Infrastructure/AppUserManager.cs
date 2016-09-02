using Lia.Blog.Domain.Entity;
using Lia.Blog.Utils;
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
    public class AppUserManager:UserManager<User>
    {
        public AppUserManager(IUserStore<User> store) : base(store)
        {

        }

        public static AppUserManager Create(IdentityFactoryOptions<AppUserManager> options, IOwinContext context)
        {
            BlogDbContext db = context.Get<BlogDbContext>();
            AppUserManager manager = new AppUserManager(new UserStore<User>(db));

            manager.PasswordValidator = new CustomPasswordValidator { RequiredLength = 6, RequireNonLetterOrDigit = false, RequireDigit = true, RequireLowercase = true, RequireUppercase = true };
            manager.UserValidator = new CustomUserValidator(manager) { AllowOnlyAlphanumericUserNames = true, RequireUniqueEmail = true };
            return manager;
        }
    }

    public class CustomUserValidator : UserValidator<User>
    {
        public CustomUserValidator(AppUserManager manager) : base(manager)
        { }

        public override async Task<IdentityResult> ValidateAsync(User user)
        {
            IdentityResult result = await base.ValidateAsync(user);
            if(!Util.IsEmail(user.Email))
            {
                var errors = result.Errors.ToList();
                errors.Add("Email Formate Error");
                result = new IdentityResult(errors);
            }
            if (string.IsNullOrEmpty(user.UserName) || user.UserName.Length < 3 || user.UserName.Length > 20)
            {
                var errors = result.Errors.ToList();
                errors.Add("UserName length must be 3 to 20");
                result = new IdentityResult(errors);
            }
            return result;
        }
    }

    public class CustomPasswordValidator : PasswordValidator
    {
        public override async Task<IdentityResult> ValidateAsync(string pass)
        {
            IdentityResult result = await base.ValidateAsync(pass);
            if (pass.Contains("12345"))
            {
                var errors = result.Errors.ToList();
                errors.Add("Passwords cannot contain numeric sequences");
                result = new IdentityResult(errors);
            }
            return result;
        }
    }
}
