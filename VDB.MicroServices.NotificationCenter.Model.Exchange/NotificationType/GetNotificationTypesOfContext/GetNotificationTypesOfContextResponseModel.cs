using System;
using System.Collections.Generic;

namespace VDB.MicroServices.NotificationCenter.Model.Exchange.NotificationType.GetNotificationTypesOfContext
{
    public record GetNotificationTypesOfContextResponseModel
    {
        /// <summary>
        /// List of notification types.
        /// </summary>
        public IEnumerable<NotificationTypeData> Types { get; set; }

        public GetNotificationTypesOfContextResponseModel(IEnumerable<NotificationTypeData> types)
        {
            Types = types;
        }
    }

    public record NotificationTypeData
    {
        /// <summary>
        /// ID of notification type.
        /// </summary>
        public Guid ID { get; set; }
        /// <summary>
        /// Type of notification.
        /// </summary>
        public string Type { get; set; }
    }
}
