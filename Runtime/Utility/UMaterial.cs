using UnityEngine;
using UnityEngine.UI;

namespace Common.Materials
{
    public static class UMaterial
    {
        public static Material GetMaterial(Transform target, int depth = 0)
        {
            if (target.TryGetComponent<Graphic>(out var graphic))
            {   // Applies to UI
                return graphic.material;
            }

            if (target.TryGetComponent<Renderer>(out var renderer))
            {   // Applies to non-UI
                return renderer.sharedMaterial ?? renderer.sharedMaterials[0];
            }

            if (depth != 0)
            {
                foreach (Transform child in target)
                {
                    if (!child.TryGetComponent<MaterialBlocker>(out _))
                    {
                        var result = GetMaterial(child, depth - 1);
                        if (result != null)
                        {
                            return result;
                        }
                    }
                }
            }

            return null;
        }

        public static void SetMaterial(Material material, Transform target, int depth = 0)
        {
            if (target.TryGetComponent<Graphic>(out var graphic))
            {   // Apply to UI
                graphic.material = material;
            }
            
            if (target.TryGetComponent<Renderer>(out var renderer))
            {   // Apply to non-UI
                renderer.sharedMaterial = material;
            }

            if (depth != 0)
            {
                foreach (Transform child in target)
                {
                    if (!child.TryGetComponent<MaterialBlocker>(out _))
                    {
                        SetMaterial(material, child, depth - 1);
                    }
                }
            }
        }
    }
}