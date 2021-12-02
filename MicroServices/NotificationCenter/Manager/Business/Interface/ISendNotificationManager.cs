using System.Threading.Tasks;
using VDB.MicroServices.NotificationCenter.Model.Exchange.SendNotification;

namespace VDB.MicroServices.NotificationCenter.Manager.Business.Interface
{
    public interface ISendNotificationManager
    {
        public Task SendNotification(SendNotificationRequestModel request);
    }
}
