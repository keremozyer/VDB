using System;
using System.Collections.Generic;

namespace VDB.MicroServices.NotificationCenter.Model.Exchange.NotificationType.GetNotificationTypesOfContext
{
    public record GetNotificationTypesOfContextResponseModel(IEnumerable<NotificationTypeData> Types);

    public record NotificationTypeData(Guid ID, string Type);
}
