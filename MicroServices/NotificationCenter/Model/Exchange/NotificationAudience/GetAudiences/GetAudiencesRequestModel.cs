using System;

namespace VDB.MicroServices.NotificationCenter.Model.Exchange.NotificationAudience.GetAudiences
{
    public record GetAudiencesRequestModel
    {
        /// <summary>
        /// NotificationContexts id.
        /// </summary>
        public Guid ContextID { get; set; }
        /// <summary>
        /// NotificationTypes id.
        /// </summary>
        public Guid TypeID { get; set; }
    }
}
