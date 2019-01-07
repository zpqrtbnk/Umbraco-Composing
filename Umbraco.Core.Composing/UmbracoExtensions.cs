using System;
using System.Linq;

namespace Umbraco.Core.Composing
{
    public static class UmbracoExtensions
    {
        public static int InvariantIndexOf(this string s, string value)
        {
            return s.IndexOf(value, StringComparison.OrdinalIgnoreCase);
        }

        public static bool IsAssignableFromGtd(this Type type, Type c)
        {
            // type *can* be a generic type definition
            // c is a real type, cannot be a generic type definition

            if (type.IsGenericTypeDefinition == false)
                return type.IsAssignableFrom(c);

            if (c.IsInterface == false)
            {
                var t = c;
                while (t != typeof(object))
                {
                    if (t.IsGenericType && t.GetGenericTypeDefinition() == type) return true;
                    t = t.BaseType;
                }
            }

            return c.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == type);
        }
    }
}
