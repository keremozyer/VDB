using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VDB.Architecture.Concern.ExtensionMethods;
using VDB.MicroServices.NotificationCenter.Manager.Business.Factory;
using VDB.MicroServices.NotificationCenter.Manager.Business.Interface;
using VDB.MicroServices.NotificationCenter.Manager.Operation.Interface;
using VDB.MicroServices.NotificationCenter.Model.Entity.POCO;
using VDB.MicroServices.NotificationCenter.Model.Exchange.SendNotification;

namespace VDB.MicroServices.NotificationCenter.Manager.Business.Implementation
{
    public class SendNotificationManager : ISendNotificationManager
    {
        private readonly INotificationContextOperations NotificationContextOperations;
        private readonly NotifierFactory NotifierFactory;

        public SendNotificationManager(INotificationContextOperations notificationContextOperations, NotifierFactory notifierFactory)
        {
            this.NotificationContextOperations = notificationContextOperations;
            this.NotifierFactory = notifierFactory;
        }

        public async Task SendNotification(SendNotificationRequestModel request)
        {
            NotificationContext context = await this.NotificationContextOperations.GetNotificationContextByType(request.NotificationContext, i => i.NotificationTypes.SelectMany(n => n.NotificationAudiences), i => i.NotificationAudiences);

            if (context.NotificationAudiences.IsNullOrEmpty()) return;

            foreach (NotificationType notificationType in context.NotificationTypes?.Where(n => n.NotificationAudiences.HasElements()) ?? new List<NotificationType>())
            {
                await this.NotifierFactory.GetNotifier(notificationType.Type).Notify(request, notificationType.NotificationAudiences.Intersect(context.NotificationAudiences));
            }
        }
    }
}
