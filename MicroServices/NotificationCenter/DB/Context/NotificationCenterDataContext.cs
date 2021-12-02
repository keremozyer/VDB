using Microsoft.EntityFrameworkCore;
using VDB.Architecture.Data.Context;
using VDB.MicroServices.NotificationCenter.Model.Entity.POCO;

namespace VDB.MicroServices.NotificationCenter.DB.Context
{
    public class NotificationCenterDataContext : BaseEFDataContext
    {
        public NotificationCenterDataContext(DbContextOptions options) : base(options) { }

        public DbSet<NotificationAudience> NotificationAudiences { get; set; }
        public DbSet<NotificationContext> NotificationContexts { get; set; }
        public DbSet<NotificationType> NotificationTypes { get; set; }
    }
}
