using UnityEngine;

namespace Common.Materials
{
    internal static class Extensions
    {
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
