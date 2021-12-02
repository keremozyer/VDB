using System.Threading.Tasks;
using VDB.MicroServices.CVEData.Model.Entity.POCO;

namespace VDB.MicroServices.CVEData.Manager.Operation.Interface
{
    public interface IProductOperations
    {
        public Task<Product> GetProductByProductAndVendorName(string productName, string vendorName);
        void Create(Product product);
    }
}
