using System.Collections.Generic;
using VDB.Architecture.Model.Entity;

namespace VDB.MicroServices.NotificationCenter.Model.Entity.POCO
{
    public class NotificationAudience : SoftDeletedEntity
    {
        public virtual List<NotificationContext> NotificationContexts { get; set; }
        public virtual List<NotificationType> NotificationTypes { get; set; }
        public string Receiver { get; set; }
    }
}
