using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using IM.Identity.BI.Enums;
using Microsoft.AspNet.Identity.Owin;

namespace IM.Identity.Web.Controllers
{
    [Authorize(Roles = RoleConstants.AdminRoles)]
    public class BaseAccountController : Controller
    {
        private ApplicationUserManager _userManager;

        public ApplicationUserManager UserManager
        {
            get { return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>(); }
            private set { _userManager = value; }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && UserManager != null)
            {
                UserManager.Dispose();
                UserManager = null;
            }

            base.Dispose(disposing);
        }

        public async Task<bool> AuthorizeAdminUser(string userId)
        {
            var hasAccess = await UserManager.IsInRoleAsync(userId, RoleConstants.SuperAdminRole) ||
                await UserManager.IsInRoleAsync(userId, RoleConstants.AdminRole);

            return hasAccess;
        }
    }
}