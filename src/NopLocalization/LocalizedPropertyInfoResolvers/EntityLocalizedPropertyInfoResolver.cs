using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;

namespace NopLocalization.Internal
{
    public class EntityLocalizedPropertyInfoResolver : IEntityLocalizedPropertyInfoResolver
    {
        #region Fields
        private static readonly ConcurrentDictionary<Type, EntityLocalizedPropertyInfo> _propertyCache = new ConcurrentDictionary<Type, EntityLocalizedPropertyInfo>();
        #endregion

        #region Methods
        public EntityLocalizedPropertyInfo GetLocalizedPropertyInfo<T>()
        {
            return GetLocalizedPropertyInfo(typeof(T));
        }

        public EntityLocalizedPropertyInfo GetLocalizedPropertyInfo(Type type)
        {
            if (!type.IsInheritFrom<ILocalizable>())
                throw new InvalidOperationException($"Type {type.Name} is not inherit from {nameof(ILocalizable)}");

            return _propertyCache.GetOrAdd(type, _ =>
            {
                var result = new EntityLocalizedPropertyInfo();

                var props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(p => p.CanRead && p.CanWrite);
                foreach (var prop in props)
                {
                    if (prop.HasAttribute<LocalizedAttribute>(true))
                    {
                        result.LocalizedProperties.Add(prop);
                    }
                    else if (prop.PropertyType.IsClass && !prop.PropertyType.IsAbstract && prop.PropertyType.IsInheritFrom<ILocalizable>())
                    {
                        result.AssosicatedProperties.Add(prop);
                    }
                    else if (!prop.PropertyType.IsAbstract && prop.PropertyType.IsEnumerableOf<ILocalizable>())
                    {
                        var elementType = prop.PropertyType.GetElementTypeOf();
                        if (elementType.IsClass && !elementType.IsAbstract && elementType.IsInheritFrom<ILocalizable>())
                            result.CollectionProperties.Add(prop);
                    }
                }

                return result;
            });
        }
        #endregion
    }
}
