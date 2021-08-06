using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using VDB.Architecture.AppException.Model;
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

        /// <summary>
        /// Returns audiences of given context and notification type.
        /// If given context or notification type is not found in db will return an Http 400 response.
        /// </summary>
        /// <param name="request">Request body</param>
        /// <returns>An object containing audiences. </returns>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetAudiencesResponseModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ParsedException))]
        [HttpGet]
        public async Task<IActionResult> GetAudiences([FromQuery] GetAudiencesRequestModel request)
        {
            return Ok(await this.NotificationContextManager.GetAudiences(request));
        }

        /// <summary>
        /// Assigns given receiver to given notification context and notification type.
        /// If given context or notification type is not found in db will return an Http 400 response.
        /// This is an idempotent service. If given receiver is already assigned to notification context and notification type duo service will return success without doing anything.
        /// </summary>
        /// <param name="request">Request body.</param>
        /// <returns>Returns no data on success.</returns>
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ParsedException))]
        [HttpPost]
        public async Task<IActionResult> AssignAudience(AssignAudienceRequestModel request)
        {
            await this.NotificationContextManager.AssignAudience(request);
            await this.DB.SaveAsync();

            return Ok();
        }
    }
}
