using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using VDB.MicroServices.NotificationCenter.DB.UnitOfWork;
using VDB.MicroServices.NotificationCenter.Manager.Operation.Interface;
using VDB.MicroServices.NotificationCenter.Model.Entity.POCO;

namespace VDB.MicroServices.NotificationCenter.Manager.Operation.Implementation
{
    public class NotificationTypeOperations : INotificationTypeOperations
    {
        private readonly INotificationCenterUnitOfWork DB;

        public NotificationTypeOperations(INotificationCenterUnitOfWork db)
        {
            this.DB = db;
        }

        public async Task<NotificationType> GetAsync(Guid id, params Expression<Func<NotificationType, object>>[] includes)
        {
            return await this.DB.NotificationTypeRepository.GetFirstAsync(n => n.Id == id, includes: includes);
        }

        public void Update(NotificationType notificationType)
        {
            this.DB.NotificationTypeRepository.Update(notificationType);
        }
    }
}
