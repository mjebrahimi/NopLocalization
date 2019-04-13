using System;

namespace NopLocalization.Internal
{
    public interface IModelLocalizedPropertyResolver
    {
        ModelLocalizedPropertyInfo GetLocalizedPropertyInfo(Type type);
        ModelLocalizedPropertyInfo GetLocalizedPropertyInfo<T>();
    }
}