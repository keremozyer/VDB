using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using VDB.MicroServices.NotificationCenter.DB.UnitOfWork;
using VDB.MicroServices.NotificationCenter.Manager.Business.Interface;
using VDB.MicroServices.NotificationCenter.Model.Exchange.NotificationAudience.AssignAudience;
using VDB.MicroServices.NotificationCenter.Model.Exchange.NotificationAudience.GetAudiences;

namespace VDB.MicroServices.NotificationCenter.Web.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class NotificationAudienceController : ControllerBase
    {
        private readonly INotificationCenterUnitOfWork DB;
        private readonly INotificationContextManager NotificationContextManager;

        public NotificationAudienceController(INotificationCenterUnitOfWork db, INotificationContextManager notificationContextManager)
        {
            this.DB = db;
            this.NotificationContextManager = notificationContextManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetAudiences([FromQuery] GetAudiencesRequestModel request)
        {
            return Ok(await this.NotificationContextManager.GetAudiences(request));
        }

        [HttpPost]
        public async Task<IActionResult> AssignAudience(AssignAudienceRequestModel request)
        {
            await this.NotificationContextManager.AssignAudience(request);
            await this.DB.SaveAsync();

            return Ok();
        }
    }
}
