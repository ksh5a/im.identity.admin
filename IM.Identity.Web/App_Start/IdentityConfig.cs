using System;
using System.Configuration;
using System.Threading.Tasks;
using IM.Identity.BI.Edm;
using IM.Identity.BI.Enums;
using IM.Identity.BI.Models;
using IM.Identity.BI.Repository.NInject;
using IM.Identity.Email.Services.Interface;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Ninject;

namespace IM.Identity.Web
{
    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.

    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser> store)
            : base(store)
        {
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context) 
        {
            var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(context.Get<ApplicationDbContext>()));
            
            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<ApplicationUser>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };
            
            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true,
            };

            // Configure user lockout defaults
            int maxFailedAccessAttemptsBeforeLockout;
            int.TryParse(ConfigurationManager.AppSettings["MaxFailedAccessAttemptsBeforeLockout"], out maxFailedAccessAttemptsBeforeLockout);

            int defaultAccountLockoutTimeSpan;
            int.TryParse(ConfigurationManager.AppSettings["DefaultAccountLockoutTimeSpan"], out defaultAccountLockoutTimeSpan);
            
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(defaultAccountLockoutTimeSpan);
            manager.MaxFailedAccessAttemptsBeforeLockout = maxFailedAccessAttemptsBeforeLockout;

            // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
            // You can write your own provider and plug in here.
            manager.RegisterTwoFactorProvider("PhoneCode", new PhoneNumberTokenProvider<ApplicationUser>
            {
                MessageFormat = "Your security code is: {0}"
            });

            manager.RegisterTwoFactorProvider("EmailCode", new EmailTokenProvider<ApplicationUser>
            {
                Subject = "Security Code",
                BodyFormat = "Your security code is: {0}"
            });

            var kernel = new StandardKernel(new RepositoryModule());
            var messageServiceManager = kernel.Get<IMessageServiceManager>();
            manager.EmailService = messageServiceManager.GetEmailService();
            manager.SmsService = messageServiceManager.GetSmsService();

            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"));
            }

            return manager;
        }

        public async Task<bool> AuthorizeAdminUser(string userId)
        {
            var authorized = await IsInRoleAsync(userId, RoleConstants.SuperAdminRole) ||
                await IsInRoleAsync(userId, RoleConstants.AdminRole);

            return authorized;
        }
    }
}
