using VDB.MicroServices.NotificationCenter.DB.UnitOfWork;
using VDB.MicroServices.NotificationCenter.Manager.Operation.Interface;
using VDB.MicroServices.NotificationCenter.Model.Entity.POCO;

namespace VDB.MicroServices.NotificationCenter.Manager.Operation.Implementation
{
    public class NotificationAudienceOperations : INotificationAudienceOperations
    {
        private readonly INotificationCenterUnitOfWork DB;

        public NotificationAudienceOperations(INotificationCenterUnitOfWork db)
        {
            this.DB = db;
        }

        public void Create(NotificationAudience audience)
        {
            this.DB.NotificationAudienceRepository.Create(audience);
        }
    }
}
