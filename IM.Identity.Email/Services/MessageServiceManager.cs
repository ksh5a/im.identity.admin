using System.Configuration;
using IM.Identity.Email.Services.Interface;
using Microsoft.AspNet.Identity;

namespace IM.Identity.Email.Services
{
    public class MessageServiceManager : IMessageServiceManager
    {
        public IIdentityMessageService GetEmailService()
        {
            var emailService = ConfigurationManager.AppSettings["EmailService"];

            switch (emailService)
            {
                case "Smtp":
                    return new SmtpEmailService();

                case "SendGrid":
                    return new SendGridEmailService();

                default:
                    return new SmtpEmailService();
            }
        }

        public IIdentityMessageService GetSmsService()
        {
            var smsService = ConfigurationManager.AppSettings["SmsService"];

            switch (smsService)
            {
                case "Twilio":
                    return new TwilioSmsService();

                default:
                    return new TwilioSmsService();
            }
        }
    }
}
