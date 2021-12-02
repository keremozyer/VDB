using System.Collections.Generic;
using System.ComponentModel;
using VDB.Architecture.Model.Entity;

namespace VDB.MicroServices.NotificationCenter.Model.Entity.POCO
{
    [DisplayName("Bildirim Bağlamı")]
    public class NotificationContext : SoftDeletedEntity
    {
        public string Type { get; set; }        
        public virtual List<NotificationType> NotificationTypes { get; set; }
        public virtual List<NotificationAudience> NotificationAudiences { get; set; }
    }
}
