using VDB.Architecture.Data.Repository.Concrete.EntityFramework;
using VDB.MicroServices.NotificationCenter.DB.Context;
using VDB.MicroServices.NotificationCenter.Model.Entity.POCO;

namespace VDB.MicroServices.NotificationCenter.DB.Repository.EntityFramework
{
    public class NotificationTypeRepository : EFSoftDeleteRepository<NotificationCenterDataContext, NotificationType>
    {
        public NotificationTypeRepository(NotificationCenterDataContext dataContext) : base(dataContext) { }
    }
}
