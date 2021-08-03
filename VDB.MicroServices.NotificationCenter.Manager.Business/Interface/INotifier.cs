using System.Collections.Generic;
using System.Threading.Tasks;
using VDB.MicroServices.NotificationCenter.Model.Entity.POCO;
using VDB.MicroServices.NotificationCenter.Model.Exchange.SendNotification;

namespace VDB.MicroServices.NotificationCenter.Manager.Business.Interface
{
    public interface INotifier
    {
        Task Notify(SendNotificationRequestModel request, IEnumerable<NotificationAudience> audiences);
    }
}
