using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
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

        [HttpPut]
        public async Task<IActionResult> Create(CreateVendorRequestModel request)
        {
            CreateVendorResponseModel response = await this.VendorBusinessManager.CreateVendor(request);
            await this.DB.SaveAsync();
            return Ok(response);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id)
        {
            await this.VendorBusinessManager.DeleteVendor(id);
            await this.DB.SaveAsync();
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> Search([FromQuery] SearchVendorsRequestModel request)
        {
            return Ok(await this.VendorBusinessManager.Search(request));
        }
    }
}
