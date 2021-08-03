using System.Linq;
using System.Threading.Tasks;
using VDB.MicroServices.CVEData.DB.UnitOfWork;
using VDB.MicroServices.CVEData.Manager.Operation.Interface;
using VDB.MicroServices.CVEData.Model.Entity.POCO;

namespace VDB.MicroServices.CVEData.Manager.Operation.Implementation
{
    public class CVEDownloadLogOperations : ICVEDownloadLogOperations
    {
        private readonly ICVEDataUnitOfWork DB;

        public CVEDownloadLogOperations(ICVEDataUnitOfWork db)
        {
            this.DB = db;
        }

        public void Create(CVEDownloadLog log)
        {
            this.DB.CVEDownloadLogRepository.Create(log);
        }

        public async Task<CVEDownloadLog> GetLatestDownloadLog()
        {
            return await this.DB.CVEDownloadLogRepository.GetFirstAsync(filter: null, orderBy: o => o.OrderByDescending(c => c.CVEDataTimestamp));
        }
    }
}
