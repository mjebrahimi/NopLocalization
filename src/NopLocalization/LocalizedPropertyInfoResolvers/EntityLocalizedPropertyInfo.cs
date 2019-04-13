using System.Collections.Generic;
using System.Reflection;

namespace NopLocalization.Internal
{
    public class EntityLocalizedPropertyInfo
    {
        public List<PropertyInfo> LocalizedProperties { get; set; } = new List<PropertyInfo>();
        public List<PropertyInfo> AssosicatedProperties { get; set; } = new List<PropertyInfo>();
        public List<PropertyInfo> CollectionProperties { get; set; } = new List<PropertyInfo>();
    }
}
