using VDB.Architecture.Data.Context;
using VDB.Architecture.Model.Entity;

namespace VDB.Architecture.Data.Repository.Concrete.EntityFramework
{
    public abstract class EFHardDeleteRepository<Context, Entity> : EFRepository<Context, Entity> where Context : BaseEFDataContext where Entity : HardDeletedEntity
    {
        protected EFHardDeleteRepository(Context dataContext) : base(dataContext) { }

        /// <summary>
        /// Executes an sql DELETE query.
        /// </summary>
        /// <param name="entity"></param>
        public override void Delete(Entity entity)
        {
            dbSet.Remove(entity);
        }
    }
}
