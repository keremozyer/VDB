using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using VDB.MicroServices.InventoryManager.DB.UnitOfWork;
using VDB.MicroServices.InventoryManager.Manager.Business.Interface;
using VDB.MicroServices.InventoryManager.Model.Exchange.Server.AssignProductVersionToServer;
using VDB.MicroServices.InventoryManager.Model.Exchange.Server.Create;
using VDB.MicroServices.InventoryManager.Model.Exchange.Server.Search;

namespace VDB.MicroServices.InventoryManager.Web.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ServerController : ControllerBase
    {
        private readonly IInventoryManagerUnitOfWork DB;
        private readonly IServerBusinessManager ServerBusinessManager;

        public ServerController(IInventoryManagerUnitOfWork db, IServerBusinessManager serverBusinessManager)
        {
            this.DB = db;
            this.ServerBusinessManager = serverBusinessManager;
        }

        [HttpPut]
        public async Task<IActionResult> Create(CreateServerRequestModel request)
        {
            CreateServerResponseModel response = await this.ServerBusinessManager.CreateServer(request);
            await this.DB.SaveAsync();
            return Ok(response);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id)
        {
            await this.ServerBusinessManager.DeleteServer(id);
            await this.DB.SaveAsync();
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> AssignProductVersion(AssignProductVersionToServerRequestModel request)
        {
            await this.ServerBusinessManager.AssignProductVersionToServer(request);
            await this.DB.SaveAsync();
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> Search([FromQuery] SearchServersRequestModel request)
        {
            return Ok(await this.ServerBusinessManager.Search(request));
        }

        [HttpGet(template: "Products/{serverID}")]
        public async Task<IActionResult> GetProductsOnServer(Guid serverID)
        {
            return Ok(await this.ServerBusinessManager.GetProductsOnServer(serverID));
        }

        [HttpDelete(template:"Remove/{serverID}/{productVersionID}")]
        public async Task<IActionResult> RemoveProductVersionFromServer(Guid serverID, Guid productVersionID)
        {
            await this.ServerBusinessManager.RemoveProductVersionFromServer(serverID, productVersionID);
            await this.DB.SaveAsync();
            return Ok();
        }
    }
}
