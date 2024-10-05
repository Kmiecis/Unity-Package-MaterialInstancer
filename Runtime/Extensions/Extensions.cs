using System.Collections.Generic;
using UnityEngine;

namespace Common.Materials
{
    internal static class Extensions
    {
        #region Array
        public static T RevokeAt<T>(this IList<T> self, int index)
        {
            var result = self[index];
            self.RemoveAt(index);
            return result;
        }
        #endregion

        #region Renderer
        public static void SetSharedMaterials(this Renderer self, List<Material> materials)
        {
            self.sharedMaterials = materials.ToArray();
        }
        #endregion

        #region Object
        public static void Destroy(this Object self)
        {
            if (Application.isPlaying)
            {
                Object.Destroy(self);
            }
            else
            {
                Object.DestroyImmediate(self);
            }
        }
        #endregion

        #region Transform
        public static int GetDepth(this Transform self, int depth = 0)
        {
            var result = depth;
            foreach (Transform child in self)
            {
                result = Mathf.Max(result, child.GetDepth(depth + 1));
            }
            return result;
        }
        #endregion
    }
}
