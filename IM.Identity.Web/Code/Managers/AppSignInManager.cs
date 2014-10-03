using System.Security.Claims;
using System.Threading.Tasks;
using IM.Identity.BI.Models;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;

namespace IM.Identity.Web.Code.Managers
{
    public class AppSignInManager : SignInManager<ApplicationUser, string>
    {
        public AppSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(ApplicationUser user)
        {
            return user.GenerateUserIdentityAsync((ApplicationUserManager)UserManager);
        }

        public static AppSignInManager Create(IdentityFactoryOptions<AppSignInManager> options, IOwinContext context)
        {
            return new AppSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication);
        }
    }
}