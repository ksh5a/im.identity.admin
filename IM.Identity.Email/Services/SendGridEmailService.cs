using System;
using System.Configuration;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using SendGrid;

namespace IM.Identity.Email.Services
{
    public class SendGridEmailService : IIdentityMessageService
    {
        public async Task SendAsync(IdentityMessage message)
        {
            var myMessage = new SendGridMessage();
            myMessage.AddTo(message.Destination);
            myMessage.From = new System.Net.Mail.MailAddress(ConfigurationManager.AppSettings["MailAdmin"]);
            myMessage.Subject = message.Subject;
            myMessage.Text = message.Body;
            myMessage.Html = message.Body;

            var credentials = new NetworkCredential
            (
                ConfigurationManager.AppSettings["SendGridAccount"],
                ConfigurationManager.AppSettings["SendGridPassword"]
            );

            // Create a Web transport for sending email.
            var transportWeb = new Web(credentials);

            try
            {
                // Send the email.
                if (transportWeb != null)
                {
                    await transportWeb.DeliverAsync(myMessage);
                }
                else
                {
                    Trace.TraceError("Failed to create Web transport.");
                    await Task.FromResult(0);
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message + " SendGrid probably not configured correctly.");
            }
        }
    }

}
