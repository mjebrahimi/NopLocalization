using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace NopLocalization.Internal
{
    public static class Utility
    {
        public static bool HasValue(this string value, bool ignoreWhiteSpace = true)
        {
            return ignoreWhiteSpace ? !string.IsNullOrWhiteSpace(value) : !string.IsNullOrEmpty(value);
        }

        public static void NotNull<T>(this T obj, string name, string message = null)
            where T : class
        {
            if (obj is null)
                throw new ArgumentNullException($"{name} : {typeof(T)}", message);
        }

        public static void NotNull<T>(this T? obj, string name, string message = null)
            where T : struct
        {
            if (!obj.HasValue)
                throw new ArgumentNullException($"{name} : {typeof(T)}", message);
        }

        public static string RemoveLeft(this string str, int length)
        {
            return str.Remove(0, length);
        }

        public static string RemoveRight(this string str, int length)
        {
            return str.Remove(str.Length - length);
        }

        /// <summary>
        /// Converts a value to a destination type.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="destinationType">The type to convert the value to.</param>
        /// <returns>The converted value.</returns>
        public static T ConvertTo<T>(this object value)
        {
            return (T)ConvertTo(value, typeof(T));
        }

        /// <summary>
        /// Converts a value to a destination type.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="destinationType">The type to convert the value to.</param>
        /// <returns>The converted value.</returns>
        public static object ConvertTo(this object value, Type type)
        {
            return value.ConvertTo(type, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Converts a value to a destination type.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="destinationType">The type to convert the value to.</param>
        /// <param name="culture">Culture</param>
        /// <returns>The converted value.</returns>
        public static object ConvertTo(this object value, Type destinationType, CultureInfo culture)
        {
            if (value == null)
                return null;

            //return Convert.ChangeType(value, type, CultureInfo.InvariantCulture);

            var destinationConverter = TypeDescriptor.GetConverter(destinationType);
            if (destinationConverter.CanConvertFrom(value.GetType()))
                return destinationConverter.ConvertFrom(null, culture, value);

            var sourceType = value.GetType();
            var sourceConverter = TypeDescriptor.GetConverter(sourceType);
            if (sourceConverter.CanConvertTo(destinationType))
                return sourceConverter.ConvertTo(null, culture, value, destinationType);

            if (destinationType.IsEnum && value is int)
                return Enum.ToObject(destinationType, (int)value);

            if (value is IConvertible && !destinationType.IsInstanceOfType(value))
                return Convert.ChangeType(value, destinationType, culture);

            return value;
        }

        public static PropertyInfo GetPropertyInfo<T, TPropType>(this Expression<Func<T, TPropType>> keySelector)
        {
            if (!(keySelector.Body is MemberExpression member))
                throw new ArgumentException($"Expression '{keySelector}' refers to a method, not a property.");
            if (!(member.Member is PropertyInfo propInfo))
                throw new ArgumentException($"Expression '{keySelector}' refers to a field, not a property.");
            return propInfo;
        }

        public static bool HasAttribute<T>(this MemberInfo type, bool inherit = false) where T : Attribute
        {
            return HasAttribute(type, typeof(T), inherit);
        }

        public static bool HasAttribute(this MemberInfo type, Type attribute, bool inherit = false)
        {
            return Attribute.IsDefined(type, attribute, inherit);
            //return type.IsDefined(attribute, inherit);
            //return type.GetCustomAttributes(attribute, inherit).Length > 0;
        }

        public static bool IsInheritFrom<T>(this Type type)
        {
            return IsInheritFrom(type, typeof(T));
        }

        public static bool IsInheritFrom(this Type type, Type parentType)
        {
            //the 'is' keyword do this too for values (new ChildClass() is ParentClass)
            return parentType.IsAssignableFrom(type);
        }

        public static Type GetInterface<T>(this Type type)
        {
            return type.GetInterface(typeof(T));
        }

        public static Type GetInterface(this Type type, Type interfaceType)
        {
            return type.GetInterface(interfaceType.FullName);
        }

        public static bool IsGenericType(this Type type, Type genericType)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == genericType;
        }

        public static bool IsEnumerable(this Type type)
        {
            return type != typeof(string) && type.IsInheritFrom<IEnumerable>();
        }

        public static bool IsEnumerableOf<T>(this Type type)
        {
            return type.IsInheritFrom<IEnumerable<T>>();
        }

        public static bool IsEnumerableOf(this Type type, Type ofType)
        {
            var enumerableType = typeof(IEnumerable<>).MakeGenericType(ofType);
            return type.IsInheritFrom(enumerableType);
        }

        public static bool IsCustomType(this Type type)
        {
            //return type.Assembly.GetName().Name != "mscorlib";
            return type.IsCustomValueType() || type.IsCustomReferenceType();
        }

        public static bool IsCustomValueType(this Type type)
        {
            return type.IsValueType && !type.IsPrimitive && type.Namespace?.StartsWith("System", StringComparison.Ordinal) == false;
        }

        public static bool IsCustomReferenceType(this Type type)
        {
            return !type.IsValueType && !type.IsPrimitive && type.Namespace?.StartsWith("System", StringComparison.Ordinal) == false;
        }

        public static Type GetElementTypeOf(this Type type)
        {
            // Type is Array
            // short-circuit if you expect lots of arrays 
            if (type.IsArray)
                return type.GetElementType();

            // type is IEnumerable<T>;
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                return type.GetGenericArguments()[0];
            //TIPS: typeof(List<>).GetGenericArguments().Lenght == 1 but typeof(List<>).GenericTypeArguments == 0

            // type implements/extends IEnumerable<T>;
            return type.GetInterfaces().Where(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IEnumerable<>))
              .Select(t => t.GenericTypeArguments[0]).FirstOrDefault();
        }

        /// <summary>
        /// Get unproxied entity type
        /// </summary>
        /// <remarks> If your Entity Framework context is proxy-enabled, 
        /// the runtime will create a proxy instance of your entities, 
        /// i.e. a dynamically generated class which inherits from your entity class 
        /// and overrides its virtual properties by inserting specific code useful for example 
        /// for tracking changes and lazy loading.
        /// </remarks>
        /// <param name="type">Type</param>
        /// <returns></returns>
        public static Type GetUnproxiedEntityType(this Type type)
        {
            return type.IsProxy() ? type.BaseType : type;
        }

        /// <summary>
        /// Check whether an entity is proxy
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <returns>Result</returns>
        public static bool IsProxy(this Type type)
        {
            //in EF 6 we could use ObjectContext.GetObjectType. Now it's not available. Here is a workaround:
            return type.BaseType != null && type.Namespace == "Castle.Proxies";
            //other conditions
            //type.Module.ScopeName == "RefEmit_InMemoryManifestModule"
            //type.Assembly.FullName.StartsWith("DynamicProxyGenAssembly2")
        }
    }
}