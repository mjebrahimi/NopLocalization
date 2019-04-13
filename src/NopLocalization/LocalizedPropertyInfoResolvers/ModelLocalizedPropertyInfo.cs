using System.Collections.Generic;
using System.Reflection;

namespace NopLocalization.Internal
{
    public class ModelLocalizedPropertyInfo
    {
        public List<ModelLocalizedPropertyMap> LocalizedProperties { get; set; } = new List<ModelLocalizedPropertyMap>();
        public List<PropertyInfo> AssosicatedProperties { get; set; } = new List<PropertyInfo>();
        public List<PropertyInfo> CollectionProperties { get; set; } = new List<PropertyInfo>();
    }

    public class ModelLocalizedPropertyMap
    {
        public PropertyInfo EntityProperty { get; set; }
        public PropertyInfo ModelProperty { get; set; }
        public PropertyInfo ModelPropertyId { get; set; }
    }
}
