using VDB.Architecture.Data.Repository.Concrete.EntityFramework;
using VDB.MicroServices.CVEData.DB.Context;
using VDB.MicroServices.CVEData.Model.Entity.POCO;

namespace VDB.MicroServices.CVEData.DB.Repository.EntityFramework
{
    public class CPERepository : EFSoftDeleteRepository<CVEDataDataContext, CPE>
    {
        public CPERepository(CVEDataDataContext dataContext) : base(dataContext) { }
    }
}
