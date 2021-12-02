using System.Threading.Tasks;
using VDB.MicroServices.CVEData.Manager.Business.Interface;
using VDB.MicroServices.CVEData.Manager.Operation.Interface;
using VDB.MicroServices.CVEData.Model.Entity.POCO;

namespace VDB.MicroServices.CVEData.Manager.Business.Implementation
{
    public class VendorBusinessManager : IVendorBusinessManager
    {
        private readonly IVendorOperations VendorOperations;

        public VendorBusinessManager(IVendorOperations vendorOperations)
        {
            this.VendorOperations = vendorOperations;
        }

        public async Task<Vendor> GetOrCreateVendor(string vendorName)
        {
            Vendor vendor = await this.VendorOperations.GetVendorByName(vendorName);
            return vendor ?? CreateVendor(vendorName);
        }

        private Vendor CreateVendor(string vendorName)
        {
            Vendor vendor = new()
            {
                Name = vendorName
            };

            this.VendorOperations.Create(vendor);

            return vendor;
        }
    }
}
