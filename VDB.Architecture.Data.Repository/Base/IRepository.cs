using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using VDB.Architecture.Model.Entity;

namespace VDB.Architecture.Data.Repository.Base
{
    public interface IRepository<Entity> where Entity : BaseEntity
    {
        void Create(Entity entity);
        void Update(Entity entity);
        void Delete(Entity entity);
        Task<List<Entity>> GetAsync(Expression<Func<Entity, bool>> filter, Func<IQueryable<Entity>, IOrderedQueryable> orderBy = null, int pageCount = 0, int pageSize = 0, params Expression<Func<Entity, object>>[] includes);
        Task<Entity> GetFirstAsync(Expression<Func<Entity, bool>> filter, Func<IQueryable<Entity>, IOrderedQueryable> orderBy = null, params Expression<Func<Entity, object>>[] includes);
        Task<int> GetCount(Expression<Func<Entity, bool>> filter);
    }
}
