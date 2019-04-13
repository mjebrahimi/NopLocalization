using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;

namespace NopLocalization.Internal
{
    public class ModelLocalizedPropertyResolver : IModelLocalizedPropertyResolver
    {
        #region Fields
        private static readonly ConcurrentDictionary<Type, ModelLocalizedPropertyInfo> _propertyCache = new ConcurrentDictionary<Type, ModelLocalizedPropertyInfo>();
        #endregion

        #region Methods
        public ModelLocalizedPropertyInfo GetLocalizedPropertyInfo<T>()
        {
            return GetLocalizedPropertyInfo(typeof(T));
        }

        public ModelLocalizedPropertyInfo GetLocalizedPropertyInfo(Type type)
        {
            var iLocalizedModel = type.GetInterface(typeof(ILocalizedModel<,>));
            if (iLocalizedModel == null)
                throw new InvalidOperationException($"Type {type.Name} is not inherit from {typeof(ILocalizedModel<,>).Name}");

            return _propertyCache.GetOrAdd(type, _ =>
            {
                var entityType = iLocalizedModel.GetGenericArguments().First();
                var props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(p => p.CanRead && p.CanWrite);

                var result = new ModelLocalizedPropertyInfo();
                foreach (var prop in props)
                {
                    if (!prop.PropertyType.IsAbstract && !prop.PropertyType.IsCustomReferenceType() && !prop.PropertyType.IsEnumerable())
                    {
                        //Localized Property
                        var localizedPropertyMap = GetLocalizedPropertyMap(prop, prop.Name, entityType);
                        if (localizedPropertyMap != null)
                            result.LocalizedProperties.Add(localizedPropertyMap);
                    }
                    else if (prop.PropertyType.IsClass && !prop.PropertyType.IsAbstract && prop.PropertyType.GetInterface(typeof(ILocalizedModel<,>)) != null)
                    {
                        result.AssosicatedProperties.Add(prop);
                    }
                    else if (!prop.PropertyType.IsAbstract && prop.PropertyType.IsEnumerable())
                    {
                        var elementType = prop.PropertyType.GetElementTypeOf();
                        if (!elementType.IsAbstract && elementType.GetInterface(typeof(ILocalizedModel<,>)) != null)
                        result.CollectionProperties.Add(prop);
                    }
                }
                return result;
            });
        }

        private ModelLocalizedPropertyMap GetLocalizedPropertyMap(PropertyInfo modelProperty, string trimedName, Type entityType)
        {
            var props = entityType.GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(p => p.CanRead && p.CanWrite);
            foreach (var prop in props)
            {
                if (trimedName.StartsWith(prop.Name, StringComparison.OrdinalIgnoreCase))
                {
                    var str = trimedName.RemoveLeft(prop.Name.Length); //CategoryName + removeLeft(Category) => Name

                    if (str == "" && prop.HasAttribute<LocalizedAttribute>(true) && entityType.IsInheritFrom<ILocalizable>())
                    {
                        var propIdName = modelProperty.Name.RemoveRight(prop.Name.Length) + "Id"; //CategoryName + removeRight(Name) => Category + Id => CategoryId

                        var propId = modelProperty.ReflectedType.GetProperty(propIdName, BindingFlags.Public | BindingFlags.Instance);
                        if (propId == null)
                            throw new Exception($"Type '{modelProperty.ReflectedType.Name}' has not property '{propIdName}'");

                        return new ModelLocalizedPropertyMap
                        {
                            EntityProperty = prop,
                            ModelPropertyId = propId,
                            ModelProperty = modelProperty
                        };
                    }

                    if (prop.PropertyType.IsCustomReferenceType())
                        return GetLocalizedPropertyMap(modelProperty, str, prop.PropertyType);
                }
            }
            return null;
        }
        #endregion
    }
}
