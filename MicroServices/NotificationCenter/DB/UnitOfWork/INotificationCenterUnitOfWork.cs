using VDB.Architecture.Data.Repository.Base;
using VDB.Architecture.Data.UnitOfWork;
using VDB.MicroServices.NotificationCenter.Model.Entity.POCO;

namespace VDB.MicroServices.NotificationCenter.DB.UnitOfWork
{
    public interface INotificationCenterUnitOfWork : IBaseUnitOfWork
    {
        IRepository<NotificationAudience> NotificationAudienceRepository { get; }
        IRepository<NotificationContext> NotificationContextRepository { get; }
        IRepository<NotificationType> NotificationTypeRepository { get; }
    }
}
