using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using VDB.MicroServices.InventoryManager.DB.UnitOfWork;
using VDB.MicroServices.InventoryManager.Manager.Operation.Interface;
using VDB.MicroServices.InventoryManager.Model.Entity.POCO;

namespace VDB.MicroServices.InventoryManager.Manager.Operation.Implementation
{
    public class ProductOperations : IProductOperations
    {
        private readonly IInventoryManagerUnitOfWork DB;

        public ProductOperations(IInventoryManagerUnitOfWork db)
        {
            this.DB = db;
        }

        public void Create(Product product)
        {
            this.DB.ProductRepository.Create(product);
        }

        public void Delete(Product product)
        {
            this.DB.ProductRepository.Delete(product);
        }

        public void Update(Product product)
        {
            this.DB.ProductRepository.Update(product);
        }

        public async Task<Product> GetProductAsync(Guid id, params Expression<Func<Product, object>>[] includes)
        {
            return await this.DB.ProductRepository.GetFirstAsync(p => p.Id == id, includes: includes);
        }

        public async Task<Product> GetProductByVendorAndNameAsync(Guid vendorId, string name)
        {
            return await this.DB.ProductRepository.GetFirstAsync(p => p.Vendor.Id == vendorId && p.Name.ToLower() == name.ToLower());
        }

        /// <summary>
        /// Searches db for products starting with given name. Makes case invariant comparison.
        /// </summary>
        /// <param name="name">Product name</param>
        /// <param name="includes">Other entities to include in search.</param>
        /// <returns>List of products.</returns>
        public async Task<List<Product>> SearchByName(string name, params Expression<Func<Product, object>>[] includes)
        {
            return await this.DB.ProductRepository.GetAsync(p => p.Name.ToLower() == name.ToLower(), includes: includes);
        }

        public async Task<List<Product>> GetAllProducts(params Expression<Func<Product, object>>[] includes)
        {
            return await this.DB.ProductRepository.GetAsync(filter: null, includes: includes);
        }

        public async Task<List<Product>> GetProductsOfVendorAsync(Guid vendorID)
        {
            return await this.DB.ProductRepository.GetAsync(p => p.VendorId == vendorID);
        }
    }
}
