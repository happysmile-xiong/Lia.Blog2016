using Lia.Blog.Application.Interfaces;
using Lia.Blog.Infrastructure;
using Lia.Blog.Infrastructure.Interfaces;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lia.Blog.Application;
using Lia.Blog.Domain.IRepository;
using Lia.Blog.Repository;

namespace Lia.Blog.Web
{
    public class ServiceLocatorInitializer
    {
        public static void Init()
        {
            IUnityContainer container = new UnityContainer();
            
            container.RegisterType<IDbContext, BlogDbContext>();
            container.RegisterType<IUnitOfWork, UnitOfWork>();

            container.RegisterType<IUserRepository, UserRepository>();
            container.RegisterType<IUserService, UserService>();


            var locator = new UnityServiceLocator(container);
            ServiceLocator.SetLocatorProvider(()=>locator);
        }
    }
}