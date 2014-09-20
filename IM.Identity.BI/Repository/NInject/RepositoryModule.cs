using IM.Identity.BI.Models;
using IM.Identity.BI.Repository.Interface;
using IM.Identity.Email.Services;
using IM.Identity.Email.Services.Interface;
using Microsoft.AspNet.Identity.EntityFramework;
using Ninject.Modules;

namespace IM.Identity.BI.Repository.NInject
{
    public class RepositoryModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IIdentityRepository<IdentityRole>>().To<RolesRepository>();
            Bind<IUserIdentityRepository<ApplicationUser>>().To<UsersRepository>();
            Bind<IRoleIdentityRepository<IdentityRole>>().To<RolesRepository>();

            Bind<IMessageServiceManager>().To<MessageServiceManager>();
        }
    }
}