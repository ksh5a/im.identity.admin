using System.Configuration;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Twilio;

namespace IM.Identity.Email.Services
{
    public class TwilioSmsService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            var twilio = new TwilioRestClient(
               ConfigurationManager.AppSettings["TwilioSid"],
               ConfigurationManager.AppSettings["TwilioToken"]
            );

            var result = twilio.SendMessage(
                ConfigurationManager.AppSettings["TwilioFromPhone"],
                message.Destination, message.Body);

            // Status is one of Queued, Sending, Sent, Failed or null if the number is not valid
            Trace.TraceInformation(result.Status);

            // Twilio doesn't currently have an async API, so return success.
            return Task.FromResult(0);
        }
    }
}
