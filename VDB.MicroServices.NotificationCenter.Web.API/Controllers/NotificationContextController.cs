using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using VDB.MicroServices.NotificationCenter.Manager.Business.Interface;

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

        [HttpGet]
        public async Task<IActionResult> GetContexes()
        {
            return Ok(await this.NotificationContextManager.GetContexes());
        }
    }
}
