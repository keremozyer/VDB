using System;

namespace VDB.MicroServices.NotificationCenter.Model.Exchange.NotificationAudience.AssignAudience
{
    public record AssignAudienceRequestModel
    {
        /// <summary>
        /// NotificationContexts id.
        /// </summary>
        public Guid ContextID { get; set; }
        /// <summary>
        /// NotificationTypes id.
        /// </summary>
        public Guid TypeID { get; set; }
        /// <summary>
        /// Receiver tag of audience.
        /// </summary>
        public string Receiver { get; set; }
    }
}
