using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace LLM.Services
{
    /// <summary>
    ///     Utility class for converting values to specific types, including enums and generic lists.
    /// </summary>
    public static class ReflectionExtensions
    {
        /// <summary>
        ///     Converts an object to the specified target type, handling enums and generic lists.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="targetType">The type to convert the value to.</param>
        /// <returns>The converted value as an object.</returns>
        public static object Cast(this Type type, object data) {
            if (type.IsInstanceOfType(data)) {
                return data;
            }

            try {
                return Convert.ChangeType(data, type);
            }
            catch (InvalidCastException) {
                var srcType = data.GetType();
                var dataParam = Expression.Parameter(srcType, "data");
                Expression body = Expression.Convert(Expression.Convert(dataParam, srcType), type);

                var run = Expression.Lambda(body, dataParam).Compile();
                return run.DynamicInvoke(data);
            }
        }
        
        /// <summary>
        /// Checks if the specified attribute is present on the provider.
        /// </summary>
        /// <typeparam name="T">The attribute to test.</typeparam>
        /// <param name="provider">The attribute provider.</param>
        /// <param name="searchInherited">If base declarations should be searched.</param>
        /// <returns>True if the attribute is present, otherwise false.</returns>
        public static bool HasAttribute<T>(this ICustomAttributeProvider provider, bool searchInherited = true) where T : Attribute {
            try {
                return provider.IsDefined(typeof(T), searchInherited);
            }
            catch (MissingMethodException) {
                return false;
            }
        }
        
        public static List<MemberInfo> GetMemberInfos<T>(this T data, BindingFlags bindingFlags) where T : class
        {
            return data.GetType()
                .GetMembers(bindingFlags)
                .Where(member => member.MemberType == MemberTypes.Field || member.MemberType == MemberTypes.Property)
                .ToList();
        }
        
        public static Dictionary<string, object> GetObjectByAttribute<TData>(TData data, Type attributeType) where TData : class
        {
            return data.GetType()
                .GetFields(BindingFlags.Public | BindingFlags.Instance)
                .Where(field => field.IsDefined(attributeType, false))
                .ToDictionary(field => field.Name, field => field.GetValue(data));
        }

        public static Dictionary<string, Type> GetTypesByAttribute<TData>(TData data, Type attributeType)
            where TData : class
        {
            return data.GetType()
                .GetFields(BindingFlags.Public | BindingFlags.Instance)
                .Where(field => field.IsDefined(attributeType, false))
                .ToDictionary(field => field.Name, field => field.FieldType);
        }
    }
}