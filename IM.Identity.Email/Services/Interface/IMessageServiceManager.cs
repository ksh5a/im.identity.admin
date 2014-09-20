using Microsoft.AspNet.Identity;

namespace IM.Identity.Email.Services.Interface
{
    public interface IMessageServiceManager
    {
        IIdentityMessageService GetEmailService();
        IIdentityMessageService GetSmsService();
    }
}
