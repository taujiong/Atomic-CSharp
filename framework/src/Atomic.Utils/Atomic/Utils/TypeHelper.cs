using System;
using JetBrains.Annotations;

namespace Atomic.Utils
{
    public static class TypeHelper
    {
        [CanBeNull]
        public static object GetDefaultValue(Type type)
        {
            return type.IsValueType ? Activator.CreateInstance(type) : null;
        }

        public static bool IsDefaultValue([CanBeNull] object obj)
        {
            return obj == null || obj.Equals(GetDefaultValue(obj.GetType()));
        }
    }
}