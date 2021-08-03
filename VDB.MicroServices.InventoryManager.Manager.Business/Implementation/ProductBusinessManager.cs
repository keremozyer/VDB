using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using VDB.MicroServices.InventoryManager.Manager.Business.Interface;
using VDB.MicroServices.InventoryManager.Manager.Operation.Interface;
using VDB.MicroServices.InventoryManager.Model.Entity.POCO;
using VDB.MicroServices.InventoryManager.Model.Exchange.Product._Common;
using VDB.MicroServices.InventoryManager.Model.Exchange.Product.Create;
using VDB.MicroServices.InventoryManager.Model.Exchange.Product.Search;
using VDB.MicroServices.InventoryManager.Model.Exchange.ProductVersion._Common;
using VDB.MicroServices.InventoryManager.Model.Exchange.Server._Common;
using VDB.Architecture.Concern.GenericValidator;
using VDB.Architecture.Concern.Resources.ResourceKeys;
using VDB.Architecture.AppException.Model.Derived.DataNotFound;
using VDB.Architecture.Concern.ExtensionMethods;

namespace VDB.MicroServices.InventoryManager.Manager.Business.Implementation
{
    public class ProductBusinessManager : IProductBusinessManager
    {
        private readonly Validator Validator;
        private readonly IMapper Mapper;
        private readonly IVendorOperations VendorOperations;
        private readonly IProductOperations ProductOperations;
        private readonly IProductVersionBusinessManager ProductVersionBusinessManager;

        public ProductBusinessManager(Validator validator, IMapper mapper, IVendorOperations vendorOperations, IProductOperations productOperations, IProductVersionBusinessManager productVersionBusinessManager)
        {
            this.Validator = validator;
            this.Mapper = mapper;
            this.VendorOperations = vendorOperations;
            this.ProductOperations = productOperations;
            this.ProductVersionBusinessManager = productVersionBusinessManager;
        }

        /// <summary>
        /// Searches db for given vendors product name (with lowercase comparasion) and creates a new product if given name is not found.
        /// </summary>
        /// <param name="request">Product data.</param>
        /// <returns>Data of found or created product.</returns>
        public async Task<CreateProductResponseModel> CreateProduct(CreateProductRequestModel request)
        {
            this.Validator.Validate<CreateProductRequestModel>(request);

            Vendor vendor = await this.VendorOperations.GetVendorAsync(request.VendorID);
            if (vendor == null) throw new DataNotFoundException(entityName: typeof(Vendor).GetDisplayName(), value: request.VendorID);

            Product product = await this.ProductOperations.GetProductByVendorAndNameAsync(vendor.Id, request.Name);
            if (product == null)
            {
                product = this.Mapper.Map<Product>(request);
                product.Vendor = vendor;
                product.Name = product.Name.ToLower();
                this.ProductOperations.Create(product);
            }

            return this.Mapper.Map<CreateProductResponseModel>(product);
        }

        /// <summary>
        /// Deletes product with given id. If product does not exists in db throws an exception.
        /// </summary>
        /// <param name="id">Product to delete</param>
        /// <returns></returns>
        public async Task DeleteProduct(Guid id)
        {
            Product product = await this.ProductOperations.GetProductAsync(id, i => i.ProductVersions);
            if (product == null) throw new DataNotFoundException(entityName: typeof(Product).GetDisplayName(), value: id);

            DeleteProduct(product);
        }

        public void DeleteProduct(Product product)
        {
            if (product == null) return;

            product.ProductVersions?.ForEach(version => this.ProductVersionBusinessManager.DeleteProductVersion(version));

            this.ProductOperations.Delete(product);
        }

        /// <summary>
        /// Searches for products starting with given name. If a name is not given returns all products. Makes case invariant comparison.
        /// </summary>
        /// <param name="request">Search data.</param>
        /// <returns>List of products.</returns>
        public async Task<SearchProductsResponseModel> Search(SearchProductsRequestModel request)
        {
            List<Expression<Func<Product, object>>> includes = null;
            if (request.IncludeVersions)
            {
                Expression<Func<Product, object>> includeExpression = i => i.ProductVersions;
                if (request.IncludeContainingServers)
                {
                    includeExpression = i => i.ProductVersions.SelectMany(p => p.Servers);
                }
                includes = new List<Expression<Func<Product, object>>>() { includeExpression };
            }
            if (request.IncludeVendorData)
            {
                includes ??= new List<Expression<Func<Product, object>>>();
                includes.Add(i => i.Vendor);
            }

            List<Product> products = String.IsNullOrWhiteSpace(request.Name) ? await this.ProductOperations.GetAllProducts(includes: includes?.ToArray()) : await this.ProductOperations.SearchByName(request.Name, includes: includes?.ToArray());

            List<ProductData> response = products?.Select(product => this.Mapper.Map<ProductData>(product))?.ToList();
            foreach (ProductData product in response ?? new List<ProductData>())
            {
                foreach (ProductVersionData version in product.ProductVersions ?? new List<ProductVersionData>())
                {
                    foreach (ServerData server in version.Servers ?? new List<ServerData>())
                    {
                        server.ProductVersions = null;
                    }
                }
            }

            return new SearchProductsResponseModel(response);
        }

        public async Task<SearchProductsResponseModel> GetProductsOfVendor(Guid vendorID)
        {
            List<Product> products = await this.ProductOperations.GetProductsOfVendorAsync(vendorID);

            return new SearchProductsResponseModel(products?.Select(product => this.Mapper.Map<ProductData>(product)));
        }
    }
}
