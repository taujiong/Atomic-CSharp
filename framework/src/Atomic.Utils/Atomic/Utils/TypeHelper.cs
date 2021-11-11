using System;

namespace Atomic.Utils
{
    public static class TypeHelper
    {
        public static object? GetDefaultValue(Type type)
        {
            return type.IsValueType ? Activator.CreateInstance(type) : null;
        }

        public static bool IsDefaultValue(object? obj)
        {
            return obj == null || obj.Equals(GetDefaultValue(obj.GetType()));
        }
    }
}