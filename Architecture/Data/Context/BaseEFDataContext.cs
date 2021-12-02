using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Linq;
using System.Threading.Tasks;
using VDB.Architecture.Data.Context.Extensions;
using VDB.Architecture.Model.Entity;

namespace VDB.Architecture.Data.Context
{
    public abstract class BaseEFDataContext : DbContext, IBaseDataContext
    {
        protected BaseEFDataContext(DbContextOptions options) : base(options) { }

        public async Task<int> SaveAsync()
        {
            return await SaveChangesAsync();
        }

        public int GetTrackedEntityCount()
        {
            return this.ChangeTracker.Entries().Count();
        }

        public void ClearChangeTrakcer()
        {
            ChangeTracker.Clear();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (IMutableEntityType entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(SoftDeletedEntity).IsAssignableFrom(entityType.ClrType))
                {
                    entityType.AddSoftDeleteQueryFilter();
                }
            }
        }
    }
}
