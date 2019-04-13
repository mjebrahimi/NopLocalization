using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NopLocalization.Internal
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> Table { get; }
        IQueryable<T> TableNoTracking { get; }

        void Add(T entity);
        Task AddAsync(T entity, CancellationToken cancellationToken = default);
        void AddRange(IEnumerable<T> entities);
        Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);
        void Delete(T entity);
        Task DeleteAsync(T entity, CancellationToken cancellationToken = default);
        void DeleteRange(IEnumerable<T> entities);
        Task DeleteRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);
        T GetById(params object[] ids);
        Task<T> GetByIdAsync(CancellationToken cancellationToken, params object[] ids);
        Task<T> GetByIdAsync(params object[] ids);
        void SaveChanges();
        void Update(T entity);
        Task UpdateAsync(T entity, CancellationToken cancellationToken = default);
        void UpdateRange(IEnumerable<T> entities);
        Task UpdateRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);
    }
}