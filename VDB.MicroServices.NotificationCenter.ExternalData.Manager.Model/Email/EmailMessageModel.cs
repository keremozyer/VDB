using System.Collections.Generic;

namespace VDB.MicroServices.NotificationCenter.ExternalData.Manager.Model.Email
{
    public class EmailMessageModel
    {
        public IEnumerable<string> ReceiverAddresses { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public IEnumerable<AttachmentModel> Attachments { get; set; }
    }

    public class AttachmentModel
    {
        public byte[] Data { get; set; }
        public string FileName { get; set; }
        public string MediaType { get; set; }
    }
}
