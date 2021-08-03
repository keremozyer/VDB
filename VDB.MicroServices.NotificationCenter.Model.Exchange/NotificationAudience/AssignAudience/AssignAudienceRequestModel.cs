using System;

namespace VDB.MicroServices.NotificationCenter.Model.Exchange.NotificationAudience.AssignAudience
{
    public record AssignAudienceRequestModel(Guid ContextID, Guid TypeID, string Receiver);
}
