using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using VDB.MicroServices.InventoryManager.DB.UnitOfWork;
using VDB.MicroServices.InventoryManager.Manager.Business.Interface;
using VDB.MicroServices.InventoryManager.Model.Exchange.Product.Create;
using VDB.MicroServices.InventoryManager.Model.Exchange.Product.Search;

namespace VDB.MicroServices.InventoryManager.Web.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IInventoryManagerUnitOfWork InventoryManagerUnitOfWork;
        private readonly IProductBusinessManager ProductBusinessManager;

        public ProductController(IInventoryManagerUnitOfWork inventoryManagerUnitOfWork, IProductBusinessManager productBusinessManager)
        {
            this.ProductBusinessManager = productBusinessManager;
            this.InventoryManagerUnitOfWork = inventoryManagerUnitOfWork;
        }

        [HttpPut]
        public async Task<IActionResult> Create(CreateProductRequestModel request)
        {
            CreateProductResponseModel response = await this.ProductBusinessManager.CreateProduct(request);
            await this.InventoryManagerUnitOfWork.SaveAsync();
            return Ok(response);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id)
        {
            await this.ProductBusinessManager.DeleteProduct(id);
            await this.InventoryManagerUnitOfWork.SaveAsync();
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> Search([FromQuery] SearchProductsRequestModel request)
        {
            return Ok(await this.ProductBusinessManager.Search(request));
        }

        [HttpGet(template:"ByVendor/{vendorID}")]
        public async Task<IActionResult> GetProductsOfVendor(Guid vendorID)
        {
            return Ok(await this.ProductBusinessManager.GetProductsOfVendor(vendorID));
        }
    }
}
