using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using VDB.MicroServices.NotificationCenter.Manager.Business.Interface;

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

        [HttpGet]
        public async Task<IActionResult> GetNotificationTypesOfContext(Guid contextID)
        {
            return Ok(await this.NotificationContextManager.GetNotificationTypesOfContext(contextID));
        }
    }
}
