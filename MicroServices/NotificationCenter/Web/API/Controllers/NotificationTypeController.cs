using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using VDB.Architecture.AppException.Model;
using VDB.MicroServices.NotificationCenter.Manager.Business.Interface;
using VDB.MicroServices.NotificationCenter.Model.Exchange.NotificationType.GetNotificationTypesOfContext;

namespace VDB.MicroServices.NotificationCenter.Web.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class NotificationTypeController : ControllerBase
    {
        private readonly INotificationContextManager NotificationContextManager;

        public NotificationTypeController(INotificationContextManager notificationContextManager)
        {            
            this.NotificationContextManager = notificationContextManager;
        }

        /// <summary>
        /// Returns notification types under given context.
        /// Will return an http status code 400 if given context is not found in db.
        /// </summary>
        /// <param name="contextID">ID of notification context.</param>
        /// <returns>An object containing list of notification types. If types is null and http status code is 200 this means no notification types were found.</returns>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetNotificationTypesOfContextResponseModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ParsedException))]
        [HttpGet]
        public async Task<IActionResult> GetNotificationTypesOfContext(Guid contextID)
        {
            return Ok(await this.NotificationContextManager.GetNotificationTypesOfContext(contextID));
        }
    }
}
