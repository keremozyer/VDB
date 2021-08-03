using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using VDB.MicroServices.InventoryManager.DB.UnitOfWork;
using VDB.MicroServices.InventoryManager.Manager.Business.Interface;
using VDB.MicroServices.InventoryManager.Model.Exchange.ProductVersion.Create;

namespace VDB.MicroServices.InventoryManager.Web.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProductVersionController : ControllerBase
    {
        private readonly IInventoryManagerUnitOfWork DB;
        private readonly IProductVersionBusinessManager ProductVersionBusinessManager;

        public ProductVersionController(IInventoryManagerUnitOfWork db, IProductVersionBusinessManager productVersionBusinessManager)
        {
            this.DB = db;
            this.ProductVersionBusinessManager = productVersionBusinessManager;
        }

        [HttpPut]
        public async Task<IActionResult> Create(CreateProductVersionRequestModel request)
        {
            CreateProductVersionResponseModel response = await this.ProductVersionBusinessManager.CreateProductVersion(request);
            await this.DB.SaveAsync();
            return Ok(response);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id)
        {
            await this.ProductVersionBusinessManager.DeleteProductVersion(id);
            await this.DB.SaveAsync();

            return Ok();
        }

        [HttpGet(template: "ByProduct/{productID}")]
        public async Task<IActionResult> GetVersionsOfProduct(Guid productID)
        {
            return Ok(await this.ProductVersionBusinessManager.GetVersionsOfProduct(productID));
        }
    }
}
