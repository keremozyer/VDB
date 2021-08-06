using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using VDB.Architecture.AppException.Model;
using VDB.MicroServices.InventoryManager.DB.UnitOfWork;
using VDB.MicroServices.InventoryManager.Manager.Business.Interface;
using VDB.MicroServices.InventoryManager.Model.Exchange.Vendor.Create;
using VDB.MicroServices.InventoryManager.Model.Exchange.Vendor.Search;

namespace VDB.MicroServices.InventoryManager.Web.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class VendorController : ControllerBase
    {
        private readonly IInventoryManagerUnitOfWork DB;
        private readonly IVendorBusinessManager VendorBusinessManager;

        public VendorController(IInventoryManagerUnitOfWork db, IVendorBusinessManager vendorBusinessManager)
        {
            this.DB = db;
            this.VendorBusinessManager = vendorBusinessManager;
        }

        /// <summary>
        /// Creates given vendor. Converts vendor name to lowercase before creating.
        /// This is an idempotent service. If a vendor already exists service will return existing products id.        
        /// </summary>
        /// <param name="request">Request body.</param>
        /// <returns>Created vendors data.</returns>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CreateVendorResponseModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ParsedException))]
        [HttpPut]
        public async Task<IActionResult> Create(CreateVendorRequestModel request)
        {
            CreateVendorResponseModel response = await this.VendorBusinessManager.CreateVendor(request);
            await this.DB.SaveAsync();
            return Ok(response);
        }

        /// <summary>
        /// Deletes given vendor from db. If said vendor has product records deletes them and product versions assigned to those products also.
        /// If given vendor is not found in db Http 400 will be returned.
        /// </summary>
        /// <param name="id">Id of vendor to be deleted.</param>
        /// <returns>Returns no data on success.</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ParsedException))]
        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id)
        {
            await this.VendorBusinessManager.DeleteVendor(id);
            await this.DB.SaveAsync();
            return Ok();
        }

        /// <summary>
        /// Searches db with given parameters in request and returns data according to flags given in request.
        /// </summary>
        /// <param name="request">Parameters. All of them are optional.</param>
        /// <returns>An object containing list of found vendors. If vendors is null and http status is 200 this means no vendors were found.</returns>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SearchVendorsResponseModel))]
        [HttpGet]
        public async Task<IActionResult> Search([FromQuery] SearchVendorsRequestModel request)
        {
            return Ok(await this.VendorBusinessManager.Search(request));
        }
    }
}
