using System.Threading.Tasks;
using VDB.MicroServices.CVEData.Model.Entity.POCO;

namespace VDB.MicroServices.CVEData.Manager.Operation.Interface
{
    public interface IVendorOperations
    {
        public Task<Vendor> GetVendorByName(string vendorName);
        void Create(Vendor vendor);
    }
}
