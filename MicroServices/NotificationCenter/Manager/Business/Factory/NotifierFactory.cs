using System;
using VDB.MicroServices.NotificationCenter.Concern.Constants;
using VDB.MicroServices.NotificationCenter.Manager.Business.Implementation.Notifiers;
using VDB.MicroServices.NotificationCenter.Manager.Business.Interface;

namespace VDB.MicroServices.NotificationCenter.Manager.Business.Factory
{
    public class NotifierFactory
    {
        private readonly EmailNotifier EmailNotifier;

        public NotifierFactory(EmailNotifier emailNotifier)
        {
            this.EmailNotifier = emailNotifier; ;
        }

        public INotifier GetNotifier(string notificationType)
        {
            return notificationType switch
            {
                NotificationConstants.NotificationTypeEmail => this.EmailNotifier,
                _ => throw new NotImplementedException()
             };
        }
    }
}
