using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using VDB.Architecture.AppException.Model;
using VDB.MicroServices.InventoryManager.DB.UnitOfWork;
using VDB.MicroServices.InventoryManager.Manager.Business.Interface;
using VDB.MicroServices.InventoryManager.Model.Exchange.Server.AssignProductVersionToServer;
using VDB.MicroServices.InventoryManager.Model.Exchange.Server.Create;
using VDB.MicroServices.InventoryManager.Model.Exchange.Server.GetProductsOnServer;
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

        /// <summary>
        /// Creates given server. Converts server name to lowercase before creating.
        /// This is an idempotent service. If a server already exists service will return existing servers id.
        /// </summary>
        /// <param name="request">Request body.</param>
        /// <returns>Created servers data.</returns>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CreateServerResponseModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ParsedException))]
        [HttpPut]
        public async Task<IActionResult> Create(CreateServerRequestModel request)
        {
            CreateServerResponseModel response = await this.ServerBusinessManager.CreateServer(request);
            await this.DB.SaveAsync();
            return Ok(response);
        }

        /// <summary>
        /// Deletes given server from db.
        /// If given server is not found in db Http 400 will be returned.
        /// </summary>
        /// <param name="id">Id of server to be deleted.</param>
        /// <returns>Returns no data on success.</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ParsedException))]
        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id)
        {
            await this.ServerBusinessManager.DeleteServer(id);
            await this.DB.SaveAsync();
            return Ok();
        }

        /// <summary>
        /// Assigns given product version to given server.
        /// This is an idempotent service. If given product version is already assigned to given server service will return success without doing anything.
        /// If given product version or server is not found in db Http 400 will be returned.
        /// </summary>
        /// <param name="request">Request body.</param>
        /// <returns>Returns no data on success.</returns>
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ParsedException))]
        [HttpPost]
        public async Task<IActionResult> AssignProductVersion(AssignProductVersionToServerRequestModel request)
        {
            await this.ServerBusinessManager.AssignProductVersionToServer(request);
            await this.DB.SaveAsync();
            return Ok();
        }

        /// <summary>
        /// Searches db with given parameters in request and returns data according to flags given in request.
        /// </summary>
        /// <param name="request">Parameters. All of them are optional.</param>
        /// <returns>An object containing list of found servers. If servers is null and http status is 200 this means no servers were found.</returns>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SearchServersResponseModel))]
        [HttpGet]
        public async Task<IActionResult> Search([FromQuery] SearchServersRequestModel request)
        {
            return Ok(await this.ServerBusinessManager.Search(request));
        }

        /// <summary>
        /// Gets all product versions contained in given server.
        /// If given server is not found in db Http 400 will be returned.
        /// </summary>
        /// <param name="serverID">Servers id</param>
        /// <returns>An object containing product versions.</returns>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetProductVersionsOnServerResponseModel))]
        [HttpGet(template: "Products/{serverID}")]
        public async Task<IActionResult> GetProductsOnServer(Guid serverID)
        {
            return Ok(await this.ServerBusinessManager.GetProductsOnServer(serverID));
        }

        /// <summary>
        /// Removes given product version from given server.
        /// If given server is not found in db Http 400 will be returned.
        /// This is an idempotent service. If given product version already isn't contained in the server service will return success without doing anything.
        /// </summary>
        /// <param name="serverID">Servers id.</param>
        /// <param name="productVersionID">Product versions id.</param>
        /// <returns>Returns no data on success.</returns>
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ParsedException))]
        [HttpDelete(template:"Remove/{serverID}/{productVersionID}")]
        public async Task<IActionResult> RemoveProductVersionFromServer(Guid serverID, Guid productVersionID)
        {
            await this.ServerBusinessManager.RemoveProductVersionFromServer(serverID, productVersionID);
            await this.DB.SaveAsync();
            return Ok();
        }
    }
}
