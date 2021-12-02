using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using VDB.MicroServices.CVEData.Manager.Business.Interface;
using VDB.MicroServices.CVEData.Model.Exchange.CVESearch.SearchByProductVersion;

namespace VDB.MicroServices.CVEData.Web.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CVESearchController : ControllerBase
    {
        private readonly ICVESearchBusinessManager CVESearchBusinessManager;

        public CVESearchController(ICVESearchBusinessManager cveSearchBusinessManager)
        {
            this.CVESearchBusinessManager = cveSearchBusinessManager;
        }

        [HttpGet]
        public async Task<IActionResult> SearchByProductVersion([FromQuery] SearchByProductVersionRequestModel request)
        {
            return Ok(await this.CVESearchBusinessManager.SearchByProductVersion(request));
        }
    }
}
