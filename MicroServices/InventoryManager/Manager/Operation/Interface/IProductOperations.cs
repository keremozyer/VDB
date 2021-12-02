using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using VDB.MicroServices.InventoryManager.Model.Entity.POCO;

namespace VDB.MicroServices.InventoryManager.Manager.Operation.Interface
{
    public interface IProductOperations
    {
        public void Create(Product product);
        public void Delete(Product product);
        public void Update(Product product);
        public Task<Product> GetProductAsync(Guid id, params Expression<Func<Product, object>>[] includes);
        public Task<Product> GetProductByVendorAndNameAsync(Guid vendorId, string name);
        public Task<List<Product>> SearchByName(string name, params Expression<Func<Product, object>>[] includes);
        public Task<List<Product>> GetAllProducts(params Expression<Func<Product, object>>[] includes);
        public Task<List<Product>> GetProductsOfVendorAsync(Guid vendorID);
    }
}
