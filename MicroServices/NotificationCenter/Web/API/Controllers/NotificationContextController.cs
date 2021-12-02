using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using VDB.MicroServices.NotificationCenter.Manager.Business.Interface;
using VDB.MicroServices.NotificationCenter.Model.Exchange.NotificationContext.GetContexes;

namespace VDB.MicroServices.NotificationCenter.Web.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class NotificationContextController : ControllerBase
    {
        private readonly INotificationContextManager NotificationContextManager;

        public NotificationContextController(INotificationContextManager notificationContextManager)
        {            
            this.NotificationContextManager = notificationContextManager;
        }

        /// <summary>
        /// Returns all notification contexes in db.
        /// </summary>
        /// <returns>An object containing list of notification contexes. If contexes is null and http status code is 200 this means no contexes were found.</returns>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetContexesResponseModel))]
        [HttpGet]
        public async Task<IActionResult> GetContexes()
        {
            return Ok(await this.NotificationContextManager.GetContexes());
        }
    }
}
