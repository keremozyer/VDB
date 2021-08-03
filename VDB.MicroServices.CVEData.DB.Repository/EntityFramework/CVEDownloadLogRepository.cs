using VDB.Architecture.Data.Repository.Concrete.EntityFramework;
using VDB.MicroServices.CVEData.DB.Context;
using VDB.MicroServices.CVEData.Model.Entity.POCO;

namespace VDB.MicroServices.CVEData.DB.Repository.EntityFramework
{
    public class CVEDownloadLogRepository : EFHardDeleteRepository<CVEDataDataContext, CVEDownloadLog>
    {
        public CVEDownloadLogRepository(CVEDataDataContext dataContext) : base(dataContext) { }
    }
}
