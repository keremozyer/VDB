using System;
using System.ComponentModel;
using System.Reflection;

namespace VDB.Architecture.Concern.ExtensionMethods
{
    public static class TypeExtensions
    {
        public static string GetDisplayName(this Type type)
        {
            return type.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName ?? type.Name;
        }
    }
}
