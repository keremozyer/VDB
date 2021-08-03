using System;

namespace VDB.MicroServices.NotificationCenter.Model.Exchange.NotificationAudience.GetAudiences
{
    public record GetAudiencesRequestModel(Guid ContextID, Guid TypeID)
    {
    }
}
