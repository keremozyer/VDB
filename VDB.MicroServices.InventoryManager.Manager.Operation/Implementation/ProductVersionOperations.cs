using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VDB.MicroServices.InventoryManager.DB.UnitOfWork;
using VDB.MicroServices.InventoryManager.Manager.Operation.Interface;
using VDB.MicroServices.InventoryManager.Model.Entity.POCO;

namespace VDB.MicroServices.InventoryManager.Manager.Operation.Implementation
{
    public class ProductVersionOperations : IProductVersionOperations
    {
        private readonly IInventoryManagerUnitOfWork DB;

        public ProductVersionOperations(IInventoryManagerUnitOfWork db)
        {
            this.DB = db;
        }

        public void Create(ProductVersion productVersion)
        {
            this.DB.ProductVersionRepository.Create(productVersion);
        }

        public void Delete(ProductVersion productVersion)
        {
            this.DB.ProductVersionRepository.Delete(productVersion);
        }

        public async Task<ProductVersion> GetProductVersionAsync(Guid id)
        {
            return await this.DB.ProductVersionRepository.GetFirstAsync(pv => pv.Id == id);
        }

        public async Task<ProductVersion> GetProductVersionByProductAndVersionAsync(Guid productId, string version)
        {
            return await this.DB.ProductVersionRepository.GetFirstAsync(pv => pv.Product.Id == productId && pv.Version.ToLower() == version.ToLower());
        }

        public async Task<List<ProductVersion>> GetVersionsOfProduct(Guid productID)
        {
            return await this.DB.ProductVersionRepository.GetAsync(pv => pv.ProductId == productID);
        }
    }
}
