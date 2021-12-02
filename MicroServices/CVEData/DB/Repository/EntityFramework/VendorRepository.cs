using VDB.Architecture.Data.Repository.Concrete.EntityFramework;
using VDB.MicroServices.CVEData.DB.Context;
using VDB.MicroServices.CVEData.Model.Entity.POCO;

namespace VDB.MicroServices.CVEData.DB.Repository.EntityFramework
{
    public class VendorRepository : EFSoftDeleteRepository<CVEDataDataContext, Vendor>
    {
        public VendorRepository(CVEDataDataContext dataContext) : base(dataContext) { }
    }
}
