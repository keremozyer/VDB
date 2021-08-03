using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VDB.MicroServices.NotificationCenter.ExternalData.Manager.Model.Email;
using VDB.MicroServices.NotificationCenter.ExternalData.Manager.Service.Emailing;
using VDB.MicroServices.NotificationCenter.Manager.Business.Interface;
using VDB.MicroServices.NotificationCenter.Model.Entity.POCO;
using VDB.MicroServices.NotificationCenter.Model.Exchange.SendNotification;

namespace VDB.MicroServices.NotificationCenter.Manager.Business.Implementation.Notifiers
{
    public class EmailNotifier : INotifier
    {
        private readonly IEmailServiceManager EmailServiceManager;

        public EmailNotifier(IEmailServiceManager emailServiceManager)
        {
            this.EmailServiceManager = emailServiceManager;
        }

        public async Task Notify(SendNotificationRequestModel request, IEnumerable<NotificationAudience> audiences)
        {
            EmailMessageModel model = new()
            {
                Subject = request.Subject,
                Body = request.Content,
                ReceiverAddresses = audiences?.Select(a => a.Receiver),
                Attachments = request.Attachments?.Select(a => new AttachmentModel()
                {
                    Data = Convert.FromBase64String(a.Base64Data),
                    FileName = a.FileName,
                    MediaType = a.ContentType
                })
            };
            await this.EmailServiceManager.SendMail(model);
        }
    }
}
