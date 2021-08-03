using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using VDB.MicroServices.NotificationCenter.Concern.Options;
using VDB.MicroServices.NotificationCenter.ExternalData.Manager.Model.Email;

namespace VDB.MicroServices.NotificationCenter.ExternalData.Manager.Service.Emailing.GoogleMail
{
    public class GoogleEmailServiceManager : IEmailServiceManager
    {
        private readonly EmailServiceSettings EmailServiceSettings;
        private readonly EmailServiceSecrets EmailServiceSecrets;

        public GoogleEmailServiceManager(IOptions<EmailServiceSettings> emailServiceSettings, IOptions<EmailServiceSecrets> emailServiceSecrets)
        {
            this.EmailServiceSettings = emailServiceSettings.Value;
            this.EmailServiceSecrets = emailServiceSecrets.Value;
        }

        public async Task SendMail(EmailMessageModel message)
        {
            SmtpClient smtp = new(this.EmailServiceSettings.Host, this.EmailServiceSettings.Port);
            smtp.EnableSsl = true;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential(this.EmailServiceSecrets.Username, this.EmailServiceSecrets.Password);

            using MailMessage mail = new(this.EmailServiceSecrets.Username, String.Join(this.EmailServiceSettings.ToMailAddressSeperator, message.ReceiverAddresses));
            mail.From = new MailAddress(this.EmailServiceSecrets.Username);
            mail.Subject = message.Subject;
            mail.Body = message.Body;
            mail.IsBodyHtml = true;
            mail.BodyEncoding = Encoding.UTF8;
            foreach (AttachmentModel attachment in message.Attachments ?? new List<AttachmentModel>())
            {
                mail.Attachments.Add(new Attachment(new MemoryStream(attachment.Data), attachment.FileName, attachment.MediaType));
            }

            await smtp.SendMailAsync(mail);
        }
    }
}
