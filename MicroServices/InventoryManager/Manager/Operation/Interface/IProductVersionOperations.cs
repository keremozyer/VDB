using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VDB.MicroServices.InventoryManager.Model.Entity.POCO;

namespace VDB.MicroServices.InventoryManager.Manager.Operation.Interface
{
    public interface IProductVersionOperations
    {
        public void Create(ProductVersion productVersion);
        public void Delete(ProductVersion productVersion);
        public Task<ProductVersion> GetProductVersionAsync(Guid id);
        public Task<ProductVersion> GetProductVersionByProductAndVersionAsync(Guid productId, string version);
        Task<List<ProductVersion>> GetVersionsOfProduct(Guid productID);
    }
}
