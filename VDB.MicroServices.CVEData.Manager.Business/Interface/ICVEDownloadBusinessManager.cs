using System.Threading.Tasks;

namespace VDB.MicroServices.CVEData.Manager.Business.Interface
{
    public interface ICVEDownloadBusinessManager
    {
        public Task GetSystemUpToDate();        
    }
}
