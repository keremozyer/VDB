using VDB.Architecture.Data.Repository.Concrete.EntityFramework;
using VDB.MicroServices.NotificationCenter.DB.Context;
using VDB.MicroServices.NotificationCenter.Model.Entity.POCO;

namespace VDB.MicroServices.NotificationCenter.DB.Repository.EntityFramework
{
    public class NotificationAudienceRepository : EFSoftDeleteRepository<NotificationCenterDataContext, NotificationAudience>
    {
        public NotificationAudienceRepository(NotificationCenterDataContext dataContext) : base(dataContext) { }
    }
}
