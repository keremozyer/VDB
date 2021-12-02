using System.Threading.Tasks;
using VDB.MicroServices.CVEData.Model.Entity.POCO;

namespace VDB.MicroServices.CVEData.Manager.Business.Interface
{
    public interface IVendorBusinessManager
    {
        public Task<Vendor> GetOrCreateVendor(string vendorName);
    }
}
