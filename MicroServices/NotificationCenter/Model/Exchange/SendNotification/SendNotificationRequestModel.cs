using System.Collections.Generic;
using VDB.Architecture.Model.Messaging;

namespace VDB.MicroServices.NotificationCenter.Model.Exchange.SendNotification
{
    public record SendNotificationRequestModel : BaseMessagingRequest
    {
        public string NotificationContext { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public IEnumerable<NotificationAttachmentModel> Attachments { get; set; }
    }

    public record NotificationAttachmentModel
    {
        public string Base64Data { get; set; }
        public string ContentType { get; set; }
        public string FileName { get; set; }
    }
}
