using System.Collections.Generic;
using VDB.Architecture.Model.Messaging;

namespace VDB.MicroServices.NotificationCenter.Model.Exchange.CreateTicket
{
    public record CreateTicketRequestModel(IEnumerable<TicketData> Data) : BaseMessagingRequest;

    public record TicketData(string Summary, string Description, string CVEID, string Vendor, string Product, string Version);
}
