using System.Threading.Tasks;
using VDB.MicroServices.CVEData.Manager.Business.Interface;
using VDB.MicroServices.CVEData.Manager.Operation.Interface;
using VDB.MicroServices.CVEData.Model.Entity.POCO;

namespace VDB.MicroServices.CVEData.Manager.Business.Implementation
{
    public class ProductBusinessManager : IProductBusinessManager
    {
        private readonly IProductOperations ProductOperations;
        private readonly IVendorBusinessManager VendorManager;

        public ProductBusinessManager(IProductOperations productOperations, IVendorBusinessManager vendorManager)
        {
            this.ProductOperations = productOperations;
            this.VendorManager = vendorManager;
        }

        public async Task<Product> GetOrCreateProduct(string productName, string vendorName)
        {
            Product product = await this.ProductOperations.GetProductByProductAndVendorName(productName, vendorName);
            return product ?? await CreateProduct(productName, vendorName);
        }

        private async Task<Product> CreateProduct(string productName, string vendorName)
        {
            Vendor vendor = await this.VendorManager.GetOrCreateVendor(vendorName);

            Product product = new()
            {
                Name = productName,
                Vendor = vendor
            };

            this.ProductOperations.Create(product);

            return product;
        }
    }
}
