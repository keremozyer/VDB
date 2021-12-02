using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using VDB.Architecture.Concern.ExtensionMethods;
using VDB.Architecture.Data.Context;
using VDB.Architecture.Data.Repository.Base;
using VDB.Architecture.Model.Entity;

namespace VDB.Architecture.Data.Repository.Concrete.EntityFramework
{
    public abstract class EFRepository<DataContext, Entity> : IRepository<Entity> where DataContext : BaseEFDataContext where Entity : BaseEntity
    {
        protected DbSet<Entity> dbSet { get; set; }
        protected DataContext dataContext { get; set; }

        protected EFRepository(DataContext dataContext)
        {
            this.dataContext = dataContext;
            this.dbSet = this.dataContext.Set<Entity>();
        }

        private IQueryable<Entity> ApplyFilter(Expression<Func<Entity, bool>> filter)
        {
            return filter != null ? dbSet.Where(filter) : dbSet;
        }

        private static IQueryable<Entity> ApplyIncludes(IQueryable<Entity> query, IEnumerable<Expression<Func<Entity, object>>> includes)
        {
            if (includes.IsNullOrEmpty())
            {
                return query;
            }

            foreach (var include in includes)
            {
                if (include.Body.NodeType == ExpressionType.MemberAccess)
                {
                    query = query.Include(include);
                }
                else
                {
                    // Modifies include expression to simulate joins using .Select() and .SelectMany() feature of querying directly DbSet
                    string path = Regex.Replace(include.ToString(), @"\(.*?=>.*?\.", ".").Replace(".SelectMany.", ".").Replace(".Select.", ".").Replace("(", String.Empty).Replace(")", String.Empty);
                    query = query.Include(Regex.Replace(path, @".*?=>.*?\.", String.Empty));
                }
            }

            return query;
        }

        private IQueryable<Entity> ApplyExpressions(Expression<Func<Entity, bool>> filter, Func<IQueryable<Entity>, IOrderedQueryable> orderBy = null, params Expression<Func<Entity, object>>[] includes)
        {
            IQueryable<Entity> query = ApplyIncludes(ApplyFilter(filter), includes);
            if (orderBy != null)
            {
                query = (IQueryable<Entity>)orderBy(query);
            }
            
            return query;
        }

        private static IQueryable<Entity> ApplyPaging(IQueryable<Entity> query, int pageIndex, int pageSize)
        {
            if (pageIndex < 0) throw new ArgumentException("pageCount Can't Be Less Than 1.");
            if (pageSize < 0) throw new ArgumentException("pageSize Can't Be Less Than 1.");

            if (pageIndex > 0)
            {
                query = query.Skip((int)((pageIndex - 1) * pageSize));
            }
            if (pageSize > 0)
            {
                query = query.Take(pageSize);
            }

            return query;
        }

        private IEnumerable<Entity> GetResultsFromChangeTracker()
        {
            return dataContext.ChangeTracker.Entries()?.Where(e => e.State == EntityState.Added && e.Entity is Entity)?.Select(e => e.Entity)?.Cast<Entity>();
        }

        /// <summary>
        /// Executes an sql SELECT query and returns all matching rows.
        /// </summary>
        /// <param name="filter">WHERE clause expressions. Can be null.</param>
        /// <param name="orderBy">ORDER BY expression.</param>
        /// <param name="pageIndex">If greater than zero (pageCount - 1) * pageSize elements will be skipped. Can't be less than 0.</param>
        /// <param name="pageSize">If greater than zero only this much elements will be returned. Can't be less than 0.</param>
        /// <param name="includes">JOIN expressions.</param>
        /// <returns>Concrete list of entities.</returns>
        public async Task<List<Entity>> GetAsync(Expression<Func<Entity, bool>> filter, Func<IQueryable<Entity>, IOrderedQueryable> orderBy = null, int pageIndex = 0, int pageSize = 0, params Expression<Func<Entity, object>>[] includes)
        {
            List<Entity> resultsFromDB = await ApplyPaging(ApplyExpressions(filter, orderBy, includes), pageIndex, pageSize).ToListAsync();
            IEnumerable<Entity> resultsFromChangeTracker = GetResultsFromChangeTracker();
            resultsFromChangeTracker = filter != null ? resultsFromChangeTracker?.Where(filter.Compile()) : resultsFromChangeTracker;

            return resultsFromDB.ConcatSafe(resultsFromChangeTracker)?.ToList();
        }

        /// <summary>
        /// Executes an sql SELECT query and returns first element that matches the criteria.
        /// </summary>
        /// <param name="filter">WHERE clause expressions. Can be null.</param>
        /// <param name="orderBy">ORDER BY expression.</param>
        /// <param name="includes">JOIN expressions.</param>
        /// <returns>Single concrete element.</returns>
        public async Task<Entity> GetFirstAsync(Expression<Func<Entity, bool>> filter, Func<IQueryable<Entity>, IOrderedQueryable> orderBy = null, params Expression<Func<Entity, object>>[] includes)
        {
            IEnumerable<Entity> resultsFromChangeTracker = GetResultsFromChangeTracker();
            Entity resultFromChangeTracker = filter != null ? resultsFromChangeTracker?.FirstOrDefault(filter.Compile()) : resultsFromChangeTracker?.FirstOrDefault();
            if (resultFromChangeTracker != null && orderBy == null)
            {
                return resultFromChangeTracker;
            }

            Entity resultFromDB = await ApplyExpressions(filter, orderBy, includes).FirstOrDefaultAsync();

            if (resultFromChangeTracker != null && orderBy != null)
            {
                return ((IEnumerable<Entity>)orderBy.Method.Invoke((new List<Entity>() { resultFromChangeTracker, resultFromDB }), null)).First();
            }

             return resultFromDB;
        }

        public async Task<int> GetCount(Expression<Func<Entity, bool>> filter)
        {
            int countFromDB = await ApplyExpressions(filter, null, null).CountAsync();
            IEnumerable<Entity> resultsFromChangeTracker = GetResultsFromChangeTracker();
            resultsFromChangeTracker = filter != null ? resultsFromChangeTracker?.Where(filter.Compile()) : resultsFromChangeTracker;

            return countFromDB + (resultsFromChangeTracker?.Count()).GetValueOrDefault();
        }

        /// <summary>
        /// Executes an sql INSERT query.
        /// </summary>
        /// <param name="entity">Element to be inserted.</param>
        public void Create(Entity entity)
        {
            dbSet.Add(entity);
        }

        /// <summary>
        /// Executes an sql UPDATE query.
        /// </summary>
        /// <param name="entity">Element to be updated.</param>
        public void Update(Entity entity)
        {
            entity.LastUpdatedAt = DateTime.UtcNow;
            EntityEntry entry = dataContext.Entry(entity);
            EntityState currentState = entry.State;
            if (currentState != EntityState.Added)
            {
                if (currentState == EntityState.Detached)
                {
                    dbSet.Attach(entity);
                }
                entry.State = EntityState.Modified;
            }
        }

        public abstract void Delete(Entity entity);
    }
}
