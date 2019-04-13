using System;

namespace NopLocalization
{
    /// <summary>
    /// Set this attribute to multi-language properties
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class LocalizedAttribute : Attribute
    {
    }
}
