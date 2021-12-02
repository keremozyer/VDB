using System.Threading.Tasks;
using VDB.MicroServices.NotificationCenter.Model.Exchange.CreateTicket;

namespace VDB.MicroServices.NotificationCenter.Manager.Business.Interface
{
    public interface ITicketCreationBusinessManager
    {
        Task CreateTicket(CreateTicketRequestModel request);
    }
}
