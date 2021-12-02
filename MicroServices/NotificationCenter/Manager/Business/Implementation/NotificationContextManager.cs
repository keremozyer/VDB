using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using VDB.Architecture.AppException.Model.Derived.DataNotFound;
using VDB.Architecture.Concern.ExtensionMethods;
using VDB.Architecture.Concern.GenericValidator;
using VDB.MicroServices.NotificationCenter.Manager.Business.Interface;
using VDB.MicroServices.NotificationCenter.Manager.Operation.Interface;
using VDB.MicroServices.NotificationCenter.Model.Entity.POCO;
using VDB.MicroServices.NotificationCenter.Model.Exchange.NotificationAudience.AssignAudience;
using VDB.MicroServices.NotificationCenter.Model.Exchange.NotificationAudience.GetAudiences;
using VDB.MicroServices.NotificationCenter.Model.Exchange.NotificationContext.GetContexes;
using VDB.MicroServices.NotificationCenter.Model.Exchange.NotificationType.GetNotificationTypesOfContext;

namespace VDB.MicroServices.NotificationCenter.Manager.Business.Implementation
{
    public class NotificationContextManager : INotificationContextManager
    {
        private readonly Validator Validator;
        private readonly IMapper Mapper;
        private readonly INotificationContextOperations NotificationContextOperations;
        private readonly INotificationTypeOperations NotificationTypeOperations;
        private readonly INotificationAudienceOperations NotificationAudienceOperations;

        public NotificationContextManager(Validator validator, IMapper mapper, INotificationContextOperations notificationContextOperations, INotificationTypeOperations notificationTypeOperations, INotificationAudienceOperations notificationAudienceOperations)
        {
            this.Validator = validator;
            this.Mapper = mapper;
            this.NotificationContextOperations = notificationContextOperations;
            this.NotificationTypeOperations = notificationTypeOperations;
            this.NotificationAudienceOperations = notificationAudienceOperations;
        }

        public async Task<GetContexesResponseModel> GetContexes()
        {
            List<NotificationContext> contexes = await this.NotificationContextOperations.GetAllContexes();

            return new GetContexesResponseModel(contexes?.Select(c => this.Mapper.Map<NotificationContextData>(c)));
        }

        public async Task<GetNotificationTypesOfContextResponseModel> GetNotificationTypesOfContext(Guid contextID)
        {
            NotificationContext notificationContext = await this.NotificationContextOperations.GetAsync(contextID, i => i.NotificationTypes);
            if (notificationContext == null) throw new DataNotFoundException(entityName: typeof(NotificationContext).GetDisplayName(), value: contextID);

            return new GetNotificationTypesOfContextResponseModel(notificationContext.NotificationTypes?.Select(n => this.Mapper.Map<NotificationTypeData>(n)));
        }

        public async Task<GetAudiencesResponseModel> GetAudiences(GetAudiencesRequestModel request)
        {
            NotificationContext notificationContext = await this.NotificationContextOperations.GetAsync(request.ContextID, i => i.NotificationAudiences);
            if (notificationContext == null) throw new DataNotFoundException(entityName: typeof(NotificationContext).GetDisplayName(), value: request.ContextID);

            NotificationType notificationType = await this.NotificationTypeOperations.GetAsync(request.TypeID, i => i.NotificationAudiences);
            if (notificationType == null) throw new DataNotFoundException(entityName: typeof(NotificationType).GetDisplayName(), value: request.TypeID);

            return new GetAudiencesResponseModel(notificationContext.NotificationAudiences.IntersectSafe(notificationType.NotificationAudiences)?.Select(a => this.Mapper.Map<AudienceData>(a)));
        }

        public async Task<Guid> AssignAudience(AssignAudienceRequestModel request)
        {
            this.Validator.Validate<AssignAudienceRequestModel>(request);

            NotificationContext notificationContext = await this.NotificationContextOperations.GetAsync(request.ContextID, i => i.NotificationAudiences);
            if (notificationContext == null) throw new DataNotFoundException(entityName: typeof(NotificationContext).GetDisplayName(), value: request.ContextID);

            NotificationType notificationType = await this.NotificationTypeOperations.GetAsync(request.TypeID, i => i.NotificationAudiences);
            if (notificationType == null) throw new DataNotFoundException(entityName: typeof(NotificationType).GetDisplayName(), value: request.TypeID);

            NotificationAudience audience = notificationContext.NotificationAudiences.ConcatSafe(notificationType.NotificationAudiences)?.FirstOrDefault(a => a.Receiver == request.Receiver.Trim());
            if (audience == null)
            {
                audience = new NotificationAudience()
                {
                    Receiver = request.Receiver.Trim()
                };
                this.NotificationAudienceOperations.Create(audience);
            }

            if (!(notificationContext.NotificationAudiences?.Any(a => a.Id == audience.Id)).GetValueOrDefault())
            {
                notificationContext.NotificationAudiences ??= new List<NotificationAudience>();
                notificationContext.NotificationAudiences.Add(audience);

                this.NotificationContextOperations.Update(notificationContext);
            }

            if (!(notificationType.NotificationAudiences?.Any(a => a.Id == audience.Id)).GetValueOrDefault())
            {
                notificationType.NotificationAudiences ??= new List<NotificationAudience>();
                notificationType.NotificationAudiences.Add(audience);

                this.NotificationTypeOperations.Update(notificationType);
            }

            return audience.Id;
        }
    }
}
