using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using VDB.MicroServices.NotificationCenter.DB.UnitOfWork;
using VDB.MicroServices.NotificationCenter.Manager.Operation.Interface;
using VDB.MicroServices.NotificationCenter.Model.Entity.POCO;

namespace VDB.MicroServices.NotificationCenter.Manager.Operation.Implementation
{
    public class NotificationContextOperations : INotificationContextOperations
    {
        private readonly INotificationCenterUnitOfWork DB;

        public NotificationContextOperations(INotificationCenterUnitOfWork db)
        {
            this.DB = db;
        }

        public async Task<NotificationContext> GetNotificationContextByType(string type, params Expression<Func<NotificationContext, object>>[] includes)
        {
            return await this.DB.NotificationContextRepository.GetFirstAsync(c => c.Type == type, includes: includes);
        }

        public async Task<List<NotificationContext>> GetAllContexes()
        {
            return await this.DB.NotificationContextRepository.GetAsync(filter: null);
        }

        public async Task<NotificationContext> GetAsync(Guid contextID, params Expression<Func<NotificationContext, object>>[] includes)
        {
            return await this.DB.NotificationContextRepository.GetFirstAsync(c => c.Id == contextID, includes: includes);
        }

        public void Update(NotificationContext notificationContext)
        {
            this.DB.NotificationContextRepository.Update(notificationContext);
        }
    }
}
