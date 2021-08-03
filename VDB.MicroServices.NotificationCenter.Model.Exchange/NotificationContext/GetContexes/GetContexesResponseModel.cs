using System;
using System.Collections.Generic;

namespace VDB.MicroServices.NotificationCenter.Model.Exchange.NotificationContext.GetContexes
{
    public record GetContexesResponseModel(IEnumerable<NotificationContextData> Contexes);

    public record NotificationContextData(Guid ID, string Type);
}
