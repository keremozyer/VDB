using System.Collections.Generic;
using System.ComponentModel;
using VDB.Architecture.Model.Entity;

namespace VDB.MicroServices.NotificationCenter.Model.Entity.POCO
{
    [DisplayName("Bildirim Tipi")]
    public class NotificationType : SoftDeletedEntity
    {
        public virtual List<NotificationContext> NotificationContexts { get; set; }
        public virtual List<NotificationAudience> NotificationAudiences { get; set; }
        public string Type { get; set; }
    }
}
