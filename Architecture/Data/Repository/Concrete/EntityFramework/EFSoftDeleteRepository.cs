using System;
using VDB.Architecture.Data.Context;
using VDB.Architecture.Data.Repository.Base;
using VDB.Architecture.Model.Entity;

namespace VDB.Architecture.Data.Repository.Concrete.EntityFramework
{
    public abstract class EFSoftDeleteRepository<Context, Entity> : EFRepository<Context, Entity> where Context : BaseEFDataContext where Entity : SoftDeletedEntity
    {
        protected EFSoftDeleteRepository(Context dataContext) : base(dataContext) { }

        /// <summary>
        /// Sets DeletedAt timestamp of entity and calls Update method of Repository.
        /// </summary>
        /// <param name="entity"></param>
        public override void Delete(Entity entity)
        {
            entity.DeletedAt = DateTime.Now;
            Update(entity);
        }
    }
}
