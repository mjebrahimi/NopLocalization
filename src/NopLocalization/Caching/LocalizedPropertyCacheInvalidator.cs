using AutoMapper;
using CacheManager.Core;
using System.Collections.Generic;
using System.Linq;

namespace NopLocalization.Internal
{
    public class LocalizedPropertyCacheInvalidator : ILocalizedPropertyCacheInvalidator
    {
        #region Fields
        protected readonly ICacheManager<List<LocalizedPropertyCached>> _cacheManager;
        protected readonly ICacheManager<string> _cacheManagerString;
        protected readonly IMapper _mapper;
        #endregion

        #region Constructor
        public LocalizedPropertyCacheInvalidator(ICacheManager<List<LocalizedPropertyCached>> cacheManager,
            ICacheManager<string> cacheManagerString,
            IMapper mapper)
        {
            _cacheManager = cacheManager;
            _cacheManagerString = cacheManagerString;
            _mapper = mapper;
        }
        #endregion

        #region Methods
        public void InvalidateCache(LocalizedProperty localizedProperty, CacheInvalidationOperation operation)
        {
            var item = _mapper.Map<LocalizedProperty, LocalizedPropertyCached>(localizedProperty);

            var success = _cacheManager.TryUpdate(LocalizationCacheKeys.LocalizedPropertyAllCacheKey, LocalizationCacheKeys.LocalizedPropertyCacheRegion, list =>
            {
                switch (operation)
                {
                    case CacheInvalidationOperation.Add:
                        list.Add(item);
                        break;
                    case CacheInvalidationOperation.Update:
                        var updateIndex = list.FindIndex(p => p.Id == item.Id);
                        list[updateIndex] = item;
                        break;
                    case CacheInvalidationOperation.Delete:
                        var removeIndex = list.FindIndex(p => p.Id == item.Id);
                        list.RemoveAt(removeIndex);
                        break;
                }
                return list;
            }, out var _);

            if (!success)
                _cacheManager.ClearRegion(LocalizationCacheKeys.LocalizedPropertyCacheRegion);

            success = _updateCacheManagerString(item, operation);
            if (!success)
                _cacheManagerString.ClearRegion(LocalizationCacheKeys.LocalizedPropertyCacheRegion);
        }

        public void InvalidateCache(IEnumerable<LocalizedProperty> localizedProperties, CacheInvalidationOperation operation)
        {
            if (!localizedProperties.Any())
                return;

            var items = localizedProperties.Select(_mapper.Map<LocalizedProperty, LocalizedPropertyCached>);

            var success = _cacheManager.TryUpdate(LocalizationCacheKeys.LocalizedPropertyAllCacheKey, LocalizationCacheKeys.LocalizedPropertyCacheRegion, list =>
            {
                switch (operation)
                {
                    case CacheInvalidationOperation.Add:
                        list.AddRange(items);
                        break;
                    case CacheInvalidationOperation.Update:
                        foreach (var item in items)
                        {
                            var index = list.FindIndex(p => p.Id == item.Id);
                            list[index] = item;
                        }
                        break;
                    case CacheInvalidationOperation.Delete:
                        foreach (var item in items)
                        {
                            var index = list.FindIndex(p => p.Id == item.Id);
                            list.RemoveAt(index);
                        }
                        break;
                }
                return list;
            }, out var _);

            if (!success)
                _cacheManager.ClearRegion(LocalizationCacheKeys.LocalizedPropertyCacheRegion);

            foreach (var item in items)
            {
                success = _updateCacheManagerString(item, operation);
                if (!success)
                {
                    _cacheManagerString.ClearRegion(LocalizationCacheKeys.LocalizedPropertyCacheRegion);
                    return;
                }
            }
        }

        private bool _updateCacheManagerString(LocalizedPropertyCached item, CacheInvalidationOperation operation)
        {
            var key = string.Format(LocalizationCacheKeys.LocalizedPropertyCacheKey, item.EntityName, item.EntityId, item.PropertyName, item.LanguageId);
            switch (operation)
            {
                case CacheInvalidationOperation.Add:
                    return true; //No need to add item to _cacheManagerString because it is for gradual loading
                case CacheInvalidationOperation.Update:
                    return _cacheManagerString.TryUpdate(key, LocalizationCacheKeys.LocalizedPropertyCacheRegion, _ => item.LocaleValue ?? "", out var _);
                case CacheInvalidationOperation.Delete:
                    return _cacheManagerString.Remove(key, LocalizationCacheKeys.LocalizedPropertyCacheRegion);
                default:
                    throw null;
            }
        }
        #endregion
    }

    public enum CacheInvalidationOperation
    {
        Add,
        Update,
        Delete
    }
}
