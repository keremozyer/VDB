using System.Threading.Tasks;
using VDB.MicroServices.NotificationCenter.Model.Exchange.CreateTicket;

namespace VDB.MicroServices.NotificationCenter.ExternalData.Manager.Contract
{
    public interface ITicketServiceManager
    {
        Task<bool> HasTicketWithCVE(TicketData ticketData);
        Task CreateTicket(TicketData ticketData);
    }
}
