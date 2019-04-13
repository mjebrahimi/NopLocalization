using NopLocalization.Internal;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CacheManager.Core;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NopLocalization
{
    public class LanguageRepository : Repository<Language>, ILanguageRepository
    {
        #region Fields
        protected readonly ICacheManager<List<LanguageCached>> _cacheManager;
        protected readonly IConfigurationProvider _configurationProvider;
        #endregion

        #region Constructor
        public LanguageRepository(DbContext dbContext, ICacheManager<List<LanguageCached>> cacheManager, IConfigurationProvider configurationProvider)
            : base(dbContext)
        {
            _cacheManager = cacheManager;
            _configurationProvider = configurationProvider;
        }
        #endregion

        #region Async Method
        public virtual Task<List<LanguageCached>> GetAllCachedAsync(CancellationToken cancellationToken = default)
        {
            return _cacheManager.GetAsync(LocalizationCacheKeys.LanguageAllCacheKey, LocalizationCacheKeys.LanguageCacheRegion,
                () => TableNoTracking.ProjectTo<LanguageCached>(_configurationProvider).ToListAsync(cancellationToken));
        }

        public virtual async Task<LanguageCached> GetByIdCachedAsync(int id, CancellationToken cancellationToken = default)
        {
            var languages = await GetAllCachedAsync(cancellationToken);
            return languages.Find(p => p.Id == id);
        }

        public override async Task AddAsync(Language entity, CancellationToken cancellationToken = default)
        {
            await base.AddAsync(entity, cancellationToken);
            _cacheManager.ClearRegion(LocalizationCacheKeys.LanguageCacheRegion);
        }

        public override async Task AddRangeAsync(IEnumerable<Language> entities, CancellationToken cancellationToken = default)
        {
            await base.AddRangeAsync(entities, cancellationToken);
            _cacheManager.ClearRegion(LocalizationCacheKeys.LanguageCacheRegion);
        }

        public override async Task UpdateAsync(Language entity, CancellationToken cancellationToken = default)
        {
            await base.UpdateAsync(entity, cancellationToken);
            _cacheManager.ClearRegion(LocalizationCacheKeys.LanguageCacheRegion);
        }

        public override async Task UpdateRangeAsync(IEnumerable<Language> entities, CancellationToken cancellationToken = default)
        {
            await base.UpdateRangeAsync(entities, cancellationToken);
            _cacheManager.ClearRegion(LocalizationCacheKeys.LanguageCacheRegion);
        }

        public override async Task DeleteAsync(Language entity, CancellationToken cancellationToken = default)
        {
            await base.DeleteAsync(entity, cancellationToken);
            _cacheManager.ClearRegion(LocalizationCacheKeys.LanguageCacheRegion);
        }

        public override async Task DeleteRangeAsync(IEnumerable<Language> entities, CancellationToken cancellationToken = default)
        {
            await base.DeleteRangeAsync(entities, cancellationToken);
            _cacheManager.ClearRegion(LocalizationCacheKeys.LanguageCacheRegion);
        }
        #endregion

        #region Sync Methods
        public virtual List<LanguageCached> GetAllCached()
        {
            return _cacheManager.Get(LocalizationCacheKeys.LanguageAllCacheKey, LocalizationCacheKeys.LanguageCacheRegion,
                () => TableNoTracking.ProjectTo<LanguageCached>(_configurationProvider).ToList());
        }

        public virtual LanguageCached GetByIdCached(int id)
        {
            var languages =  GetAllCached();
            return languages.Find(p => p.Id == id);
        }

        public override void Add(Language entity)
        {
            base.Add(entity);
            _cacheManager.ClearRegion(LocalizationCacheKeys.LanguageCacheRegion);
        }

        public override void AddRange(IEnumerable<Language> entities)
        {
            base.AddRange(entities);
            _cacheManager.ClearRegion(LocalizationCacheKeys.LanguageCacheRegion);
        }

        public override void Update(Language entity)
        {
            base.Update(entity);
            _cacheManager.ClearRegion(LocalizationCacheKeys.LanguageCacheRegion);
        }

        public override void UpdateRange(IEnumerable<Language> entities)
        {
            base.UpdateRange(entities);
            _cacheManager.ClearRegion(LocalizationCacheKeys.LanguageCacheRegion);
        }

        public override void Delete(Language entity)
        {
            base.Delete(entity);
            _cacheManager.ClearRegion(LocalizationCacheKeys.LanguageCacheRegion);
        }

        public override void DeleteRange(IEnumerable<Language> entities)
        {
            base.DeleteRange(entities);
            _cacheManager.ClearRegion(LocalizationCacheKeys.LanguageCacheRegion);
        }
        #endregion
    }
}
