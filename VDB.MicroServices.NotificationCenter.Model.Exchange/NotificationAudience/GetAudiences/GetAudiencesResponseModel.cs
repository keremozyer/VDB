using System;
using System.Collections.Generic;

namespace VDB.MicroServices.NotificationCenter.Model.Exchange.NotificationAudience.GetAudiences
{
    public record GetAudiencesResponseModel(IEnumerable<AudienceData> Audiences);
    public record AudienceData(Guid ID, string Receiver);
}
