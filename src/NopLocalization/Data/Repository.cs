using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NopLocalization.Internal
{
    public class Repository<T> : IRepository<T> where T : class
    {
        /// <summary>
        /// Gets db context
        /// </summary>
        protected DbContext DbContext { get; }

        /// <summary>
        /// Gets an entity set
        /// </summary>
        protected DbSet<T> Entities { get; }

        /// <summary>
        /// Gets a table
        /// </summary>
        public virtual IQueryable<T> Table => Entities;

        /// <summary>
        /// Gets a table with "no tracking" enabled (EF feature) Use it only when you load record(s) only for read-only operations
        /// </summary>
        public virtual IQueryable<T> TableNoTracking => Entities.AsNoTracking();

        public Repository(DbContext dbContext)
        {
            DbContext = dbContext;
            Entities = DbContext.Set<T>();
        }

        #region Async Method
        public virtual Task<T> GetByIdAsync(params object[] ids)
        {
            return Entities.FindAsync(ids);
        }

        public virtual Task<T> GetByIdAsync(CancellationToken cancellationToken, params object[] ids)
        {
            return Entities.FindAsync(ids, cancellationToken);
        }

        public virtual async Task AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            entity.NotNull(nameof(entity));
            await Entities.AddAsync(entity, cancellationToken).ConfigureAwait(false);
            await SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }

        public virtual async Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            entities.NotNull(nameof(entities));
            await Entities.AddRangeAsync(entities, cancellationToken).ConfigureAwait(false);
            await SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }

        public virtual async Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
        {
            entity.NotNull(nameof(entity));
            Entities.Update(entity);
            await SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }

        public virtual async Task UpdateRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            entities.NotNull(nameof(entities));
            Entities.UpdateRange(entities);
            await SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }

        public virtual async Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
        {
            entity.NotNull(nameof(entity));
            Entities.Remove(entity);
            await SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }

        public virtual async Task DeleteRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            entities.NotNull(nameof(entities));
            Entities.RemoveRange(entities);
            await SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }

        protected virtual Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return DbContext.SaveChangesAsync(cancellationToken);
        }
        #endregion

        #region Sync Methods
        public virtual T GetById(params object[] ids)
        {
            return Entities.Find(ids);
        }

        public virtual void Add(T entity)
        {
            entity.NotNull(nameof(entity));
            Entities.Add(entity);
            SaveChanges();
        }

        public virtual void AddRange(IEnumerable<T> entities)
        {
            entities.NotNull(nameof(entities));
            Entities.AddRange(entities);
            SaveChanges();
        }

        public virtual void Update(T entity)
        {
            entity.NotNull(nameof(entity));
            Entities.Update(entity);
            SaveChanges();
        }

        public virtual void UpdateRange(IEnumerable<T> entities)
        {
            entities.NotNull(nameof(entities));
            Entities.UpdateRange(entities);
            SaveChanges();
        }

        public virtual void Delete(T entity)
        {
            entity.NotNull(nameof(entity));
            Entities.Remove(entity);
            SaveChanges();
        }

        public virtual void DeleteRange(IEnumerable<T> entities)
        {
            entities.NotNull(nameof(entities));
            Entities.RemoveRange(entities);
            SaveChanges();
        }

        public virtual void SaveChanges()
        {
            DbContext.SaveChanges();
        }
        #endregion
    }
}
