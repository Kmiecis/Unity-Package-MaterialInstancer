using System;
using System.Collections.Generic;
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
        public static IEnumerable<SerializedProperty> GetArrayElements(this SerializedProperty self)
        {
            for (int i = 0; i < self.arraySize; ++i)
            {
                yield return self.GetArrayElementAtIndex(i);
            }
        }
        #endregion
    }
}