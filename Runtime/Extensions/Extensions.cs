using UnityEngine;

namespace Common.Materials
{
    internal static class Extensions
    {
        #region Component
        public static bool TryGetComponentInChildren<T>(this Component self, out T component)
        {
            component = self.GetComponentInChildren<T>();
            return component != null;
        }

        public static bool TryGetComponentsInChildren<T>(this Component self, out T[] component)
        {
            component = self.GetComponentsInChildren<T>();
            return component.Length > 0;
        }

        public static bool TryGetComponentInParent<T>(this Component self, out T component)
        {
            component = self.GetComponentInParent<T>();
            return component != null;
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
                result = Mathf.Max(result, child.GetDepth(depth + 1));
            return result;
        }
        #endregion
    }
}
