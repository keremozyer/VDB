using System.Threading.Tasks;

namespace VDB.Architecture.Data.Context
{
    public interface IBaseDataContext
    {
        Task<int> SaveAsync();
        int GetTrackedEntityCount();
        void ClearChangeTrakcer();
        void Dispose();
    }
}
