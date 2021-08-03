using AutoMapper;
using VDB.MicroServices.NotificationCenter.Model.Entity.POCO;
using VDB.MicroServices.NotificationCenter.Model.Exchange.NotificationAudience.GetAudiences;
using VDB.MicroServices.NotificationCenter.Model.Exchange.NotificationContext.GetContexes;
using VDB.MicroServices.NotificationCenter.Model.Exchange.NotificationType.GetNotificationTypesOfContext;

namespace VDB.MicroServices.NotificationCenter.Manager.Mapper._AutoMapperProfiles
{
    public class NotificationCenterMappingProfile : Profile
    {
        public NotificationCenterMappingProfile()
        {
            CreateMap<NotificationContext, NotificationContextData>();
            CreateMap<NotificationType, NotificationTypeData>();
            CreateMap<NotificationAudience, AudienceData>();
        }
    }
}
