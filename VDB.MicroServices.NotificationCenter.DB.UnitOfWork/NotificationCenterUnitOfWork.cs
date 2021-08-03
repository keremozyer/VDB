using VDB.Architecture.Data.Repository.Base;
using VDB.Architecture.Data.UnitOfWork;
using VDB.MicroServices.NotificationCenter.DB.Context;
using VDB.MicroServices.NotificationCenter.DB.Repository.EntityFramework;
using VDB.MicroServices.NotificationCenter.Model.Entity.POCO;

namespace VDB.MicroServices.NotificationCenter.DB.UnitOfWork
{
    public class NotificationCenterUnitOfWork : BaseUnitOfWork, INotificationCenterUnitOfWork
    {
        public NotificationCenterUnitOfWork(NotificationCenterDataContext context) : base(context) { }

        private NotificationAudienceRepository notificationAudienceRepository { get; set; }
        public IRepository<NotificationAudience> NotificationAudienceRepository { get { if (notificationAudienceRepository == null) { notificationAudienceRepository = new NotificationAudienceRepository((NotificationCenterDataContext)context); } return notificationAudienceRepository; } }

        private NotificationContextRepository notificationContextRepository { get; set; }
        public IRepository<NotificationContext> NotificationContextRepository { get { if (notificationContextRepository == null) { notificationContextRepository = new NotificationContextRepository((NotificationCenterDataContext)context); } return notificationContextRepository; } }
        
        private NotificationTypeRepository notificationTypeRepository { get; set; }
        public IRepository<NotificationType> NotificationTypeRepository { get { if (notificationTypeRepository == null) { notificationTypeRepository = new NotificationTypeRepository((NotificationCenterDataContext)context); } return notificationTypeRepository; } }
    }
}
