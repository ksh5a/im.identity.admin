using System.Threading.Tasks;
using System.Web.Mvc;
using IM.Identity.Web.Resources;

namespace IM.Identity.Web.Code.Managers
{
    public class EmailManager
    {
        private ApplicationUserManager UserManager { set; get; }

        public EmailManager(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }

        public async Task SendConfirmationEmail(string userId, string subject, string callbackUrl)
        {
            const string confirmUrlToken = "###EmailConfirmUrl###";
            var confirmUrl = ViewResource.EmailConfirmationTemplate.Replace(confirmUrlToken, callbackUrl);
            await UserManager.SendEmailAsync(userId, subject, confirmUrl);
        }

        public async Task SendPasswordResetEmail(string userId, string subject, string callbackUrl)
        {
            const string passwordResetUrlToken = "###PasswordResetUrl###";
            var confirmUrl = ViewResource.PasswordResetTemplate.Replace(passwordResetUrlToken, callbackUrl);
            await UserManager.SendEmailAsync(userId, subject, confirmUrl);
        }
    }
}