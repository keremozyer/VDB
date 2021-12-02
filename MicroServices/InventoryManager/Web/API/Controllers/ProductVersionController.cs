using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using VDB.Architecture.AppException.Model;
using VDB.MicroServices.InventoryManager.DB.UnitOfWork;
using VDB.MicroServices.InventoryManager.Manager.Business.Interface;
using VDB.MicroServices.InventoryManager.Model.Exchange.ProductVersion.Create;
using VDB.MicroServices.InventoryManager.Model.Exchange.ProductVersion.GetVersionsOfProduct;

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

        /// <summary>
        /// Creates given product version for supplied product. Converts product version to lowercase before creating.
        /// This is an idempotent service. If a product version already exists for given product service will return existing product versions id.
        /// </summary>
        /// <param name="request">Request body.</param>
        /// <returns>Created product versions data.</returns>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CreateProductVersionResponseModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ParsedException))]
        [HttpPut]
        public async Task<IActionResult> Create(CreateProductVersionRequestModel request)
        {
            CreateProductVersionResponseModel response = await this.ProductVersionBusinessManager.CreateProductVersion(request);
            await this.DB.SaveAsync();
            return Ok(response);
        }

        /// <summary>
        /// Deletes given product version from db.
        /// If given product version is not found in db Http 400 will be returned.
        /// </summary>
        /// <param name="id">Id of product version to be deleted.</param>
        /// <returns>Returns no data on success.</returns>
        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id)
        {
            await this.ProductVersionBusinessManager.DeleteProductVersion(id);
            await this.DB.SaveAsync();

            return Ok();
        }

        /// <summary>
        /// Gets all product versions under a product.
        /// </summary>
        /// <param name="productID">Products id.</param>
        /// <returns>An object containing list of found product versions. If productVersions is null and http status is 200 this means no product versions were found.</returns>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetVersionsOfProductResponseModel))]
        [HttpGet(template: "ByProduct/{productID}")]
        public async Task<IActionResult> GetVersionsOfProduct(Guid productID)
        {
            return Ok(await this.ProductVersionBusinessManager.GetVersionsOfProduct(productID));
        }
    }
}
