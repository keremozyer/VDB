using System.Threading.Tasks;
using VDB.MicroServices.CVEData.Model.Entity.POCO;

namespace VDB.MicroServices.CVEData.Manager.Operation.Interface
{
    public interface ICVEDownloadLogOperations
    {
        public void Create(CVEDownloadLog log);
        public Task<CVEDownloadLog> GetLatestDownloadLog();
    }
}
