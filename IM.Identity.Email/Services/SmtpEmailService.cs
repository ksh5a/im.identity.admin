using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;
using IM.Identity.Email.Mime;
using Microsoft.AspNet.Identity;

namespace IM.Identity.Email.Services
{
    public class SmtpEmailService : IIdentityMessageService
    {
        private const int DefaultPort = 25;
        private const int MailMessageRetries = 5;
        private static int MailMessageRetryTimeout { get { return (int)(new TimeSpan(0, 0, 0, 5)).TotalSeconds; } }

        private string SmtpServer { get; set; }
        private NetworkCredential SmtpLogin { get; set; }
        public int Port { get; set; }

        public string From { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        public bool IsHtml { get; set; }

        public IEnumerable<AlternateView> AlternateViews { get; set; }
        public Dictionary<string, Stream> Attachments { get; set; }

        public SmtpEmailService()
        {
            SmtpServer = ConfigurationManager.AppSettings["SmtpServer"];
            SmtpLogin = new NetworkCredential
            (
                ConfigurationManager.AppSettings["SmtpUserName"], 
                ConfigurationManager.AppSettings["SmtpPassword"]
            );

            bool isHtml;
            bool.TryParse(ConfigurationManager.AppSettings["SmtpHtmlBody"], out isHtml);
            IsHtml = isHtml;

            From = ConfigurationManager.AppSettings["MailAdmin"];
            Attachments = new Dictionary<string, Stream>();
            AlternateViews = Enumerable.Empty<AlternateView>();
        }

        public Task SendAsync(IdentityMessage message)
        {
            var mailSentSuccessfully = false;
            var smtpClient = new SmtpClient();

            int smtpPort;
            var smtpPortString = ConfigurationManager.AppSettings["SmtpPort"];
            int.TryParse(smtpPortString, out smtpPort);
            smtpClient.Port = string.IsNullOrEmpty(smtpPortString) ? DefaultPort : smtpPort;

            if (!string.IsNullOrEmpty(SmtpServer) && SmtpLogin != null)
            {
                smtpClient.Host = SmtpServer;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = SmtpLogin;
                smtpClient.EnableSsl = true;
            }

            var mailMessage = ConstructEmail(message);
            var mailMessageRetries = MailMessageRetries;

            while (!mailSentSuccessfully && mailMessageRetries > 0)
            {
                try
                {
                    smtpClient.Send(mailMessage);
                    mailSentSuccessfully = true;
                }
                catch (Exception ex)
                {
                    mailSentSuccessfully = false;
                    Thread.Sleep(MailMessageRetryTimeout);
                }

                mailMessageRetries--;
            }

            return Task.FromResult(0);
        }

        private MailMessage ConstructEmail(IdentityMessage message)
        {
            var mailMessage = new MailMessage { Subject = message.Subject };
            mailMessage.IsBodyHtml = IsHtml;

            if (AlternateViews.Any())
            {
                foreach (var view in AlternateViews)
                {
                    mailMessage.AlternateViews.Add(view);
                }
            }
            else
            {
                mailMessage.Body = message.Body;
            }

            if (!string.IsNullOrEmpty(From))
            {
                mailMessage.From = new MailAddress(From);
            }

            mailMessage.To.Add(new MailAddress(message.Destination));

            foreach (var attachedFileInformation in Attachments)
            {
                var mimeType = DocumentExtensions.GetMimeTypeForFile(attachedFileInformation.Key);
                mailMessage.Attachments.Add(new Attachment(attachedFileInformation.Value, mimeType)
                {
                    Name = attachedFileInformation.Key
                });
            }

            mailMessage.IsBodyHtml = IsHtml;

            return mailMessage;
        }
    }
}
