using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using IM.Identity.BI.Enums;
using IM.Identity.Web.Code.Managers;
using Microsoft.AspNet.Identity;
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

        protected async Task<string> SendEmailConfirmation(string userId, string subject, string action, string controller)
        {
            var emailManager = new EmailManager(UserManager);
            var code = await UserManager.GenerateEmailConfirmationTokenAsync(userId);
            var callbackUrl = Url.Action(action, controller,
               new { userId = userId, code = code }, protocol: Request.Url.Scheme);

            return await emailManager.SendConfirmationEmail(userId, subject, callbackUrl);
        }

        protected void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
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
    }
}