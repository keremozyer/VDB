using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VDB.Architecture.AppException.Model.Derived.DataNotFound;
using VDB.Architecture.Concern.ExtensionMethods;
using VDB.Architecture.Concern.GenericValidator;
using VDB.MicroServices.InventoryManager.Manager.Business.Interface;
using VDB.MicroServices.InventoryManager.Manager.Operation.Interface;
using VDB.MicroServices.InventoryManager.Model.Entity.POCO;
using VDB.MicroServices.InventoryManager.Model.Exchange.ProductVersion._Common;
using VDB.MicroServices.InventoryManager.Model.Exchange.ProductVersion.Create;
using VDB.MicroServices.InventoryManager.Model.Exchange.ProductVersion.GetVersionsOfProduct;

namespace VDB.MicroServices.InventoryManager.Manager.Business.Implementation
{
    public class ProductVersionBusinessManager : IProductVersionBusinessManager
    {
        private readonly Validator Validator;
        private readonly IMapper Mapper;
        private readonly IProductOperations ProductOperations;
        private readonly IProductVersionOperations ProductVersionOperations;

        public ProductVersionBusinessManager(Validator validator, IMapper mapper, IProductOperations productOperations, IProductVersionOperations productVersionOperations)
        {
            this.Validator = validator;
            this.Mapper = mapper;
            this.ProductOperations = productOperations;
            this.ProductVersionOperations = productVersionOperations;
        }

        /// <summary>
        /// Searches db for given products version (with lowercase comparasion) and creates a new productVersion if given name is not found.
        /// </summary>
        /// <param name="request">ProductVersion data.</param>
        /// <returns>Data of found or created productVersion.</returns>
        public async Task<CreateProductVersionResponseModel> CreateProductVersion(CreateProductVersionRequestModel request)
        {
            this.Validator.Validate<CreateProductVersionRequestModel>(request);

            Product product = await this.ProductOperations.GetProductAsync(request.ProductID);
            if (product == null) throw new DataNotFoundException(entityName: typeof(Product).GetDisplayName(), value: request.ProductID);

            ProductVersion productVersion = await this.ProductVersionOperations.GetProductVersionByProductAndVersionAsync(product.Id, request.Version);
            if (productVersion == null)
            {
                productVersion = this.Mapper.Map<ProductVersion>(request);
                productVersion.Product = product;
                this.ProductVersionOperations.Create(productVersion);
            }

            return this.Mapper.Map<CreateProductVersionResponseModel>(productVersion);
        }

        /// <summary>
        /// Deletes productVersion with given id. If productVersion does not exists in db throws an exception.
        /// </summary>
        /// <param name="id">ProductVersion to delete</param>
        /// <returns></returns>
        public async Task DeleteProductVersion(Guid id)
        {
            ProductVersion productVersion = await this.ProductVersionOperations.GetProductVersionAsync(id);
            if (productVersion == null) throw new DataNotFoundException(entityName: typeof(ProductVersion).GetDisplayName(), value: id);

            productVersion.Servers = null;

            DeleteProductVersion(productVersion);
        }

        public void DeleteProductVersion(ProductVersion version)
        {
            if (version == null) return;

            this.ProductVersionOperations.Delete(version);
        }

        public async Task<GetVersionsOfProductResponseModel> GetVersionsOfProduct(Guid productID)
        {
            List<ProductVersion> versions = await this.ProductVersionOperations.GetVersionsOfProduct(productID);

            return new GetVersionsOfProductResponseModel() { ProductVersions = versions?.Select(version => this.Mapper.Map<ProductVersionData>(version))?.ToList() };
        }
    }
}
