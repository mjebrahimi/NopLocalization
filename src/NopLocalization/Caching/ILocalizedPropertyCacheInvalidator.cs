using System.Collections.Generic;

namespace NopLocalization.Internal
{
    public interface ILocalizedPropertyCacheInvalidator
    {
        void InvalidateCache(IEnumerable<LocalizedProperty> localizedProperties, CacheInvalidationOperation operation);
        void InvalidateCache(LocalizedProperty localizedProperty, CacheInvalidationOperation operation);
    }
}