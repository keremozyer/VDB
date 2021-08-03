using System.Threading.Tasks;
using VDB.MicroServices.CVEData.DB.UnitOfWork;
using VDB.MicroServices.CVEData.Manager.Operation.Interface;
using VDB.MicroServices.CVEData.Model.Entity.POCO;

namespace VDB.MicroServices.CVEData.Manager.Operation.Implementation
{
    public class ProductOperations : IProductOperations
    {
        private readonly ICVEDataUnitOfWork DB;

        public ProductOperations(ICVEDataUnitOfWork db)
        {
            this.DB = db;
        }

        public void Create(Product product)
        {
            this.DB.ProductRepository.Create(product);
        }

        public async Task<Product> GetProductByProductAndVendorName(string productName, string vendorName)
        {            
            return await this.DB.ProductRepository.GetFirstAsync(p => p.Name == productName && p.Vendor.Name == vendorName);
        }
    }
}
