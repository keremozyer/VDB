using VDB.MicroServices.NotificationCenter.Model.Entity.POCO;

namespace VDB.MicroServices.NotificationCenter.Manager.Operation.Interface
{
    public interface INotificationAudienceOperations
    {
        void Create(NotificationAudience audience);
    }
}
