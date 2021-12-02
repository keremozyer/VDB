using System;
using System.Threading.Tasks;

namespace VDB.Architecture.Data.UnitOfWork
{
    public interface IBaseUnitOfWork : IDisposable
    {
        Task<int> SaveAsync();
    }
}
