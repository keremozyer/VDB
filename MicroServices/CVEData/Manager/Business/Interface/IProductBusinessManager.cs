using System.Threading.Tasks;
using VDB.MicroServices.CVEData.Model.Entity.POCO;

namespace VDB.MicroServices.CVEData.Manager.Business.Interface
{
    public interface IProductBusinessManager
    {
        public Task<Product> GetOrCreateProduct(string productName, string vendorName);
    }
}
