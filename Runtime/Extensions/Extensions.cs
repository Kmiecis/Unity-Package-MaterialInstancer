using UnityEngine;

namespace Common.Materializer
{
    internal static class Extensions
    {
        #region Component
        public static bool TryGetComponentInChildren<T>(this Component self, out T component)
        {
            component = self.GetComponentInChildren<T>();
            return component != null;
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
    }
}
