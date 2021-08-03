using System;
using System.Threading.Tasks;
using VDB.MicroServices.NotificationCenter.Model.Exchange.NotificationAudience.AssignAudience;
using VDB.MicroServices.NotificationCenter.Model.Exchange.NotificationAudience.GetAudiences;
using VDB.MicroServices.NotificationCenter.Model.Exchange.NotificationContext.GetContexes;
using VDB.MicroServices.NotificationCenter.Model.Exchange.NotificationType.GetNotificationTypesOfContext;

namespace VDB.MicroServices.NotificationCenter.Manager.Business.Interface
{
    public interface INotificationContextManager
    {
        Task<GetContexesResponseModel> GetContexes();
        Task<GetNotificationTypesOfContextResponseModel> GetNotificationTypesOfContext(Guid contextID);
        Task<GetAudiencesResponseModel> GetAudiences(GetAudiencesRequestModel request);
        Task<Guid> AssignAudience(AssignAudienceRequestModel request);
    }
}
