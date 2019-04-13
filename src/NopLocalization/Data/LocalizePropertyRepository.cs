using NopLocalization.Internal;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CacheManager.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NopLocalization
{
    public class LocalizedPropertyRepository : Repository<LocalizedProperty>, ILocalizedPropertyRepository
    {
        #region Fields
        protected readonly ICacheManager<List<LocalizedPropertyCached>> _cacheManager;
        protected readonly ICacheManager<string> _cacheManagerString;
        protected readonly ILocalizedPropertyCacheInvalidator _localizedPropertyCacheUpdater;
        protected readonly IConfigurationProvider _configurationProvider;
        #endregion

        #region Constructor
        public LocalizedPropertyRepository(DbContext dbContext, ICacheManager<List<LocalizedPropertyCached>> cacheManager,
            ICacheManager<string> cacheManagerString,
            ILocalizedPropertyCacheInvalidator localizedPropertyCacheUpdater, 
            IConfigurationProvider configurationProvider)
            : base(dbContext)
        {
            _cacheManager = cacheManager;
            _cacheManagerString = cacheManagerString;
            _localizedPropertyCacheUpdater = localizedPropertyCacheUpdater;
            _configurationProvider = configurationProvider;
        }
        #endregion

        #region Async Method
        /// <summary>
        /// Gets all cached localized properties
        /// </summary>
        /// <returns>Cached localized properties</returns>
        public virtual Task<List<LocalizedPropertyCached>> GetAllCachedAsync(CancellationToken cancellationToken = default)
        {
            return _cacheManager.GetAsync(LocalizationCacheKeys.LocalizedPropertyAllCacheKey, LocalizationCacheKeys.LocalizedPropertyCacheRegion,
                () => TableNoTracking.ProjectTo<LocalizedPropertyCached>(_configurationProvider).ToListAsync(cancellationToken));
        }

        /// <summary>
        /// Find localized value
        /// </summary>
        /// <param name="entityName">Entity name</param>
        /// <param name="propertyName">Property name</param>
        /// <param name="entityId">Entity identifier</param>
        /// <param name="languageId">Language identifier</param>
        /// <param name="loadAllLocalizedProperties">true => load all records form cache | false => gradual loading</param>
        /// <returns>return localized value as string</returns>
        public virtual Task<string> GetLocalizedValueAsync(string entityName, string propertyName, int entityId,
            int languageId, bool loadAllLocalizedProperties, CancellationToken cancellationToken = default)
        {
            if (languageId == 0)
                throw new ArgumentOutOfRangeException(nameof(languageId), "Language ID should not be 0");

            var key = string.Format(LocalizationCacheKeys.LocalizedPropertyCacheKey, entityName, entityId, propertyName, languageId);
            return _cacheManagerString.GetAsync(key, LocalizationCacheKeys.LocalizedPropertyCacheRegion, async () =>
            {
                string localeValue;

                if (loadAllLocalizedProperties)
                {
                    //load all records (we know they are cached)
                    var source = await GetAllCachedAsync(cancellationToken);
                    localeValue = source
                        .Where(p => p.LanguageId == languageId && p.EntityId == entityId && p.EntityName == entityName && p.PropertyName == propertyName)
                        .Select(p => p.LocaleValue).FirstOrDefault();
                }
                else
                {
                    //gradual loading
                    localeValue = await TableNoTracking
                        .Where(p => p.LanguageId == languageId && p.EntityId == entityId && p.EntityName == entityName && p.PropertyName == propertyName)
                        .Select(p => p.LocaleValue).FirstOrDefaultAsync(cancellationToken);
                }

                //little hack here. nulls aren't cacheable so set it to ""
                return localeValue ?? "";
            });
        }

        /// <summary>
        /// Save localized value
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="entityName">Entity name</param>
        /// <param name="entityId">Entity identifier</param>
        /// <param name="propertyName">Property name</param>
        /// <param name="localeValue">Locale value</param>
        /// <param name="languageId">Language identifier</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="languageId"/></exception>
        public virtual async Task SaveLocalizedValueAsync<T>(string entityName, int entityId, string propertyName, T localeValue,
            int languageId, CancellationToken cancellationToken = default)
        {
            if (languageId == 0)
                throw new ArgumentOutOfRangeException(nameof(languageId), "Language ID should not be 0");

            var prop = await Table
                .Where(p => p.EntityName == entityName && p.PropertyName == propertyName && p.EntityId == entityId && p.LanguageId == languageId)
                .FirstOrDefaultAsync(cancellationToken);

            var localeValueStr = localeValue.ConvertTo<string>();
            if (prop != null)
            {
                if (localeValueStr.HasValue())
                {
                    prop.LocaleValue = localeValueStr;
                    await UpdateAsync(prop, cancellationToken);
                }
                else
                {
                    await DeleteAsync(prop, cancellationToken);
                }
            }
            else
            {
                if (localeValueStr.HasValue())
                {
                    prop = new LocalizedProperty
                    {
                        EntityId = entityId,
                        LanguageId = languageId,
                        PropertyName = propertyName,
                        EntityName = entityName,
                        LocaleValue = localeValueStr
                    };
                    await AddAsync(prop, cancellationToken);
                }
            }
        }


        public override async Task AddAsync(LocalizedProperty entity, CancellationToken cancellationToken = default)
        {
            await base.AddAsync(entity, cancellationToken);
            _localizedPropertyCacheUpdater.InvalidateCache(entity, CacheInvalidationOperation.Add);
        }

        public override async Task AddRangeAsync(IEnumerable<LocalizedProperty> entities, CancellationToken cancellationToken = default)
        {
            await base.AddRangeAsync(entities, cancellationToken);
            _localizedPropertyCacheUpdater.InvalidateCache(entities, CacheInvalidationOperation.Add);
        }

        public override async Task UpdateAsync(LocalizedProperty entity, CancellationToken cancellationToken = default)
        {
            await base.UpdateAsync(entity, cancellationToken);
            _localizedPropertyCacheUpdater.InvalidateCache(entity, CacheInvalidationOperation.Update);
        }

        public override async Task UpdateRangeAsync(IEnumerable<LocalizedProperty> entities, CancellationToken cancellationToken = default)
        {
            await base.UpdateRangeAsync(entities, cancellationToken);
            _localizedPropertyCacheUpdater.InvalidateCache(entities, CacheInvalidationOperation.Update);
        }

        public override async Task DeleteAsync(LocalizedProperty entity, CancellationToken cancellationToken = default)
        {
            await base.DeleteAsync(entity, cancellationToken);
            _localizedPropertyCacheUpdater.InvalidateCache(entity, CacheInvalidationOperation.Delete);
        }

        public override async Task DeleteRangeAsync(IEnumerable<LocalizedProperty> entities, CancellationToken cancellationToken = default)
        {
            await base.DeleteRangeAsync(entities, cancellationToken);
            _localizedPropertyCacheUpdater.InvalidateCache(entities, CacheInvalidationOperation.Delete);
        }
        #endregion

        #region Sync Methods
        /// <summary>
        /// Gets all cached localized properties
        /// </summary>
        /// <returns>Cached localized properties</returns>
        public virtual List<LocalizedPropertyCached> GetAllCached()
        {
            return _cacheManager.Get(LocalizationCacheKeys.LocalizedPropertyAllCacheKey, LocalizationCacheKeys.LocalizedPropertyCacheRegion,
                () => TableNoTracking.ProjectTo<LocalizedPropertyCached>(_configurationProvider).ToList());
        }

        /// <summary>
        /// Find localized value
        /// </summary>
        /// <param name="entityName">Entity name</param>
        /// <param name="propertyName">Property name</param>
        /// <param name="entityId">Entity identifier</param>
        /// <param name="languageId">Language identifier</param>
        /// <param name="loadAllLocalizedProperties">true => load all records form cache | false => gradual loading</param>
        /// <returns>return localized value as string</returns>
        public virtual string GetLocalizedValue(string entityName, string propertyName, int entityId,
            int languageId, bool loadAllLocalizedProperties)
        {
            if (languageId == 0)
                throw new ArgumentOutOfRangeException(nameof(languageId), "Language ID should not be 0");

            var key = string.Format(LocalizationCacheKeys.LocalizedPropertyCacheKey, entityName, entityId, propertyName, languageId);

            return _cacheManagerString.Get(key, LocalizationCacheKeys.LocalizedPropertyCacheRegion, () =>
            {
                string localeValue;

                if (loadAllLocalizedProperties)
                {
                    //load all records (we know they are cached)
                    var source = GetAllCached();
                    localeValue = source
                        .Where(p => p.LanguageId == languageId && p.EntityId == entityId && p.EntityName == entityName && p.PropertyName == propertyName)
                        .Select(p => p.LocaleValue).FirstOrDefault();
                }
                else
                {
                    //gradual loading
                    localeValue = TableNoTracking
                        .Where(p => p.LanguageId == languageId && p.EntityId == entityId && p.EntityName == entityName && p.PropertyName == propertyName)
                        .Select(p => p.LocaleValue).FirstOrDefault();
                }

                //little hack here. nulls aren't cacheable so set it to ""
                return localeValue ?? "";
            });
        }

        /// <summary>
        /// Save localized value
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="entityName">Entity name</param>
        /// <param name="entityId">Entity identifier</param>
        /// <param name="propertyName">Property name</param>
        /// <param name="localeValue">Locale value</param>
        /// <param name="languageId">Language identifier</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="languageId"/></exception>
        public virtual void SaveLocalizedValue<T>(string entityName, int entityId, string propertyName, T localeValue,
            int languageId)
        {
            if (languageId == 0)
                throw new ArgumentOutOfRangeException(nameof(languageId), "Language ID should not be 0");

            var prop = Table
                .Where(p => p.EntityName == entityName && p.PropertyName == propertyName && p.EntityId == entityId && p.LanguageId == languageId)
                .FirstOrDefault();

            var localeValueStr = localeValue.ConvertTo<string>();
            if (prop != null)
            {
                if (localeValueStr.HasValue())
                {
                    prop.LocaleValue = localeValueStr;
                    Update(prop);
                }
                else
                {
                    Delete(prop);
                }
            }
            else
            {
                if (localeValueStr.HasValue())
                {
                    prop = new LocalizedProperty
                    {
                        EntityId = entityId,
                        LanguageId = languageId,
                        PropertyName = propertyName,
                        EntityName = entityName,
                        LocaleValue = localeValueStr
                    };
                    Add(prop);
                }
            }
        }

        public override void Add(LocalizedProperty entity)
        {
            base.Add(entity);
            _localizedPropertyCacheUpdater.InvalidateCache(entity, CacheInvalidationOperation.Add);
        }

        public override void AddRange(IEnumerable<LocalizedProperty> entities)
        {
            base.AddRange(entities);
            _localizedPropertyCacheUpdater.InvalidateCache(entities, CacheInvalidationOperation.Add);
        }

        public override void Update(LocalizedProperty entity)
        {
            base.Update(entity);
            _localizedPropertyCacheUpdater.InvalidateCache(entity, CacheInvalidationOperation.Update);
        }

        public override void UpdateRange(IEnumerable<LocalizedProperty> entities)
        {
            base.UpdateRange(entities);
            _localizedPropertyCacheUpdater.InvalidateCache(entities, CacheInvalidationOperation.Update);
        }

        public override void Delete(LocalizedProperty entity)
        {
            base.Delete(entity);
            _localizedPropertyCacheUpdater.InvalidateCache(entity, CacheInvalidationOperation.Delete);
        }

        public override void DeleteRange(IEnumerable<LocalizedProperty> entities)
        {
            base.DeleteRange(entities);
            _localizedPropertyCacheUpdater.InvalidateCache(entities, CacheInvalidationOperation.Delete);
        }
        #endregion
    }
}
