using System.Threading.Tasks;
using VDB.MicroServices.CVEData.Model.DTO.CVEData;

namespace VDB.MicroServices.CVEData.Manager.Business.Interface
{
    public interface ICVEDataStorageBusinessManager
    {
        public Task StoreCVEResult(CVEResult result);
    }
}
