using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using VDB.MicroServices.NotificationCenter.Model.Entity.POCO;

namespace VDB.MicroServices.NotificationCenter.Manager.Operation.Interface
{
    public interface INotificationContextOperations
    {
        Task<NotificationContext> GetNotificationContextByType(string type, params Expression<Func<NotificationContext, object>>[] includes);
        Task<List<NotificationContext>> GetAllContexes();
        Task<NotificationContext> GetAsync(Guid contextID, params Expression<Func<NotificationContext, object>>[] includes);
        void Update(NotificationContext notificationContext);
    }
}
