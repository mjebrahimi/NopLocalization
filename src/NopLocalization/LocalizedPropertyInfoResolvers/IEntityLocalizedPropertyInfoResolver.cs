using System;

namespace NopLocalization.Internal
{
    public interface IEntityLocalizedPropertyInfoResolver
    {
        EntityLocalizedPropertyInfo GetLocalizedPropertyInfo(Type type);
        EntityLocalizedPropertyInfo GetLocalizedPropertyInfo<T>();
    }
}