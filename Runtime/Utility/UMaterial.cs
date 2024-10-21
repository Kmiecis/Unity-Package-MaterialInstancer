using UnityEngine;
using UnityEngine.UI;

namespace Common.Materials
{
    public static class UMaterial
    {
        public static Material FindMaterial(Transform target, int depth = -1)
        {
            if (target.TryGetComponent<Graphic>(out var graphic))
            {
                return graphic.material;
            }

            if (target.TryGetComponent<Renderer>(out var renderer))
            {
                return renderer.sharedMaterial;
            }

            if (depth != 0)
            {
                foreach (Transform child in target)
                {
                    if (!child.TryGetComponent<MaterialBlocker>(out _))
                    {
                        var result = FindMaterial(child, depth - 1);
                        if (result != null)
                        {
                            return result;
                        }
                    }
                }
            }

            return null;
        }
    }
}