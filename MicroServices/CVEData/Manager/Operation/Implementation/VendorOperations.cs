using System.Threading.Tasks;
using VDB.MicroServices.CVEData.DB.UnitOfWork;
using VDB.MicroServices.CVEData.Manager.Operation.Interface;
using VDB.MicroServices.CVEData.Model.Entity.POCO;

namespace VDB.MicroServices.CVEData.Manager.Operation.Implementation
{
    public class VendorOperations : IVendorOperations
    {
        private readonly ICVEDataUnitOfWork DB;

        public VendorOperations(ICVEDataUnitOfWork db)
        {
            this.DB = db;
        }

        public void Create(Vendor vendor)
        {
            this.DB.VendorRepository.Create(vendor);
        }

        public async Task<Vendor> GetVendorByName(string vendorName)
        {
            return await this.DB.VendorRepository.GetFirstAsync(p => p.Name == vendorName);
        }
    }
}
