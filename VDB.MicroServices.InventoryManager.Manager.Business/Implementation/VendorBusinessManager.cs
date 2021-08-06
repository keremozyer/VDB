using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using VDB.Architecture.AppException.Model.Derived.DataNotFound;
using VDB.Architecture.Concern.ExtensionMethods;
using VDB.Architecture.Concern.GenericValidator;
using VDB.MicroServices.InventoryManager.Manager.Business.Interface;
using VDB.MicroServices.InventoryManager.Manager.Operation.Interface;
using VDB.MicroServices.InventoryManager.Model.Entity.POCO;
using VDB.MicroServices.InventoryManager.Model.Exchange.Vendor._Common;
using VDB.MicroServices.InventoryManager.Model.Exchange.Vendor.Create;
using VDB.MicroServices.InventoryManager.Model.Exchange.Vendor.Search;

namespace VDB.MicroServices.InventoryManager.Manager.Business.Implementation
{
    public class VendorBusinessManager : IVendorBusinessManager
    {
        private readonly Validator Validator;
        private readonly IMapper Mapper;
        private readonly IVendorOperations VendorOperations;
        private readonly IProductBusinessManager ProductBusinessManager;

        public VendorBusinessManager(Validator validator, IMapper mapper, IVendorOperations vendorOperations, IProductBusinessManager productBusinessManager)
        {
            this.Validator = validator;
            this.Mapper = mapper;
            this.VendorOperations = vendorOperations;
            this.ProductBusinessManager = productBusinessManager;
        }

        /// <summary>
        /// Searches db for given vendor name (with lowercase comparasion) and creates a new vendor if given name is not found.
        /// </summary>
        /// <param name="request">Vendor data.</param>
        /// <returns>Data of found or created vendor.</returns>
        public async Task<CreateVendorResponseModel> CreateVendor(CreateVendorRequestModel request)
        {
            this.Validator.Validate<CreateVendorRequestModel>(request);

            Vendor vendor = await this.VendorOperations.GetVendorByNameAsync(request.Name);
            if (vendor == null)
            {
                vendor = this.Mapper.Map<Vendor>(request);
                vendor.Name = vendor.Name.ToLower();                
                this.VendorOperations.Create(vendor);
            }

            return this.Mapper.Map<CreateVendorResponseModel>(vendor);
        }

        /// <summary>
        /// Deletes vendor with given id. If vendor does not exists in db throws an exception.
        /// </summary>
        /// <param name="id">Vendor to delete</param>
        /// <returns></returns>
        public async Task DeleteVendor(Guid id)
        {
            Vendor vendor = await this.VendorOperations.GetVendorAsync(id, i => i.Products.SelectMany(p => p.ProductVersions));
            if (vendor == null) throw new DataNotFoundException(entityName: typeof(Vendor).GetDisplayName(), value: id);

            vendor.Products?.ForEach(product => this.ProductBusinessManager.DeleteProduct(product));

            this.VendorOperations.Delete(vendor);
        }

        /// <summary>
        /// Searches for vendors starting with given name. If a name is not given returns all vendors. Makes case invariant comparison.
        /// </summary>
        /// <param name="request">Search data.</param>
        /// <returns>List of vendors.</returns>
        public async Task<SearchVendorsResponseModel> Search(SearchVendorsRequestModel request)
        {
            Expression<Func<Vendor, object>>[] includes = null;
            if (request.IncludeProducts)
            {
                Expression<Func<Vendor, object>> includeExpression = i => i.Products.SelectMany(p => p.ProductVersions);
                if (request.IncludeContainingServers)
                {
                    includeExpression = i => i.Products.SelectMany(p => p.ProductVersions).SelectMany(pv => pv.Servers);
                }
                includes = new Expression<Func<Vendor, object>>[] { includeExpression };
            }

            List<Vendor> vendors = String.IsNullOrWhiteSpace(request.Name) ? await this.VendorOperations.GetAllVendors(includes?.ToArray()) : await this.VendorOperations.SearchByName(request.Name, includes: includes);

            return new SearchVendorsResponseModel(vendors?.Select(vendor => this.Mapper.Map<VendorData>(vendor)));
        }
    }
}
