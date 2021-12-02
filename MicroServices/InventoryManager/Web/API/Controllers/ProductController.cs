using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using VDB.Architecture.AppException.Model;
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

        /// <summary>
        /// Creates given product for supplied vendor. Converts product name to lowercase before creating.
        /// This is an idempotent service. If a product already exists for given vendor service will return existing products id.
        /// If given vendor is not found in db Http 400 will be returned.
        /// </summary>
        /// <param name="request">Request body.</param>
        /// <returns>Created products data.</returns>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CreateProductResponseModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ParsedException))]
        [HttpPut]
        public async Task<IActionResult> Create(CreateProductRequestModel request)
        {
            CreateProductResponseModel response = await this.ProductBusinessManager.CreateProduct(request);
            await this.InventoryManagerUnitOfWork.SaveAsync();
            return Ok(response);
        }

        /// <summary>
        /// Deletes given product from db. If said product has ProductVersion records deletes them also.
        /// If given product is not found in db Http 400 will be returned.
        /// </summary>
        /// <param name="id">Id of product to be deleted.</param>
        /// <returns>Returns no data on success.</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ParsedException))]
        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id)
        {
            await this.ProductBusinessManager.DeleteProduct(id);
            await this.InventoryManagerUnitOfWork.SaveAsync();
            return Ok();
        }

        /// <summary>
        /// Searches db with given parameters in request and returns data according to flags given in request.
        /// </summary>
        /// <param name="request">Parameters. All of them are optional.</param>
        /// <returns>An object containing list of found products. If products is null and http status is 200 this means no products were found.</returns>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SearchProductsResponseModel))]
        [HttpGet]
        public async Task<IActionResult> Search([FromQuery] SearchProductsRequestModel request)
        {
            return Ok(await this.ProductBusinessManager.Search(request));
        }

        /// <summary>
        /// Gets all products under a vendor.
        /// </summary>
        /// <param name="vendorID">Vendors id.</param>
        /// <returns>An object containing list of found products. If products is null and http status is 200 this means no products were found.</returns>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SearchProductsResponseModel))]
        [HttpGet(template:"ByVendor/{vendorID}")]
        public async Task<IActionResult> GetProductsOfVendor(Guid vendorID)
        {
            return Ok(await this.ProductBusinessManager.GetProductsOfVendor(vendorID));
        }
    }
}
