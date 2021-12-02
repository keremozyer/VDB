using System;
using System.Threading.Tasks;
using VDB.Architecture.Data.Context;

namespace VDB.Architecture.Data.UnitOfWork
{
    public abstract class BaseUnitOfWork : IBaseUnitOfWork
    {
        protected readonly IBaseDataContext context;
        private bool disposedValue;

        protected BaseUnitOfWork(IBaseDataContext context)
        {
            this.context = context;
        }

        public async Task<int> SaveAsync()
        {
            return await context.SaveAsync();
        }

        public void ClearChangeTrakcer()
        {
            context.ClearChangeTrakcer();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    this.context.Dispose();
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
