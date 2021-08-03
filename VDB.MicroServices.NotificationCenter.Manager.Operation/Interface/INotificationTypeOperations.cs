using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using VDB.MicroServices.NotificationCenter.Model.Entity.POCO;

namespace VDB.MicroServices.NotificationCenter.Manager.Operation.Interface
{
    public interface INotificationTypeOperations
    {
        Task<NotificationType> GetAsync(Guid typeID, params Expression<Func<NotificationType, object>>[] includes);
        void Update(NotificationType notificationType);
    }
}
