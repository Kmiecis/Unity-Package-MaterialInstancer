using System.Collections.Generic;
using UnityEditor;

namespace CommonEditor.Materials
{
    internal static class Extensions
    {
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