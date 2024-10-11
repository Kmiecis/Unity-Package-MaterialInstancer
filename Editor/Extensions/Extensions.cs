using System;
using System.Reflection;
using UnityEditor;

namespace CommonEditor.Materials
{
    internal static class Extensions
    {
        #region Array
        public static int IndexOfOrDefault<T>(this T[] self, T item, int fallback = default)
        {
            var result = Array.IndexOf(self, item);
            if (result == -1)
                result = fallback;
            return result;
        }
        #endregion

        #region SerializedProperty
        public static object GetValue(this SerializedProperty self)
        {
            var sanitizedPath = self.propertyPath.Replace(".Array.data[", "[");
            object result = self.serializedObject.targetObject;

            var elements = sanitizedPath.Split('.');
            foreach (var element in elements)
            {
                if (element.Contains("["))
                {
                    result = GetValueFromList(result, element);
                }
                else
                {
                    result = GetValueFromType(result, element);
                }
            }
            return result;
        }

        private static object GetValueFromList(object source, string subpath)
        {
            var indexBegin = subpath.IndexOf("[");
            var indexEnd = subpath.IndexOf("]");
            var strIndex = subpath.Substring(indexBegin + 1, indexEnd - (indexBegin + 1));
            var index = int.Parse(strIndex);

            var name = subpath.Substring(0, indexBegin);

            var list = GetValueFromType(source, name) as System.Collections.IList;
            if (list != null)
            {
                return list[index];
            }
            return null;
        }

        private static object GetValueFromType(object source, string name)
        {
            var type = source.GetType();
            while (type != null)
            {
                var fieldFlags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance;
                var field = type.GetField(name, fieldFlags);
                if (field != null)
                {
                    return field.GetValue(source);
                }

                var propertyFlags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase;
                var property = type.GetProperty(name, propertyFlags);
                if (property != null)
                {
                    return property.GetValue(source, null);
                }

                type = type.BaseType;
            }
            return null;
        }
        #endregion
    }
}