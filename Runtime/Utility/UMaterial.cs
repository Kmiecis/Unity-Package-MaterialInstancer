using UnityEngine;
using UnityEngine.UI;

namespace Common.Materials
{
    public static class UMaterial
    {
        public static Material GetMaterialFromGraphic(Transform target, int depth = 0)
        {
            if (target.TryGetComponent<Graphic>(out var graphic))
            {
                return graphic.material;
            }

            if (depth != 0)
            {
                foreach (Transform child in target)
                {
                    if (!child.TryGetComponent<MaterialBlocker>(out _))
                    {
                        var result = GetMaterialFromGraphic(child, depth - 1);
                        if (result != null)
                        {
                            return result;
                        }
                    }
                }
            }

            return null;
        }

        public static Material GetMaterialFromRenderer(Transform target, int depth = 0)
        {
            if (target.TryGetComponent<Renderer>(out var renderer))
            {
                return renderer.sharedMaterial ?? renderer.sharedMaterials[0];
            }

            if (depth != 0)
            {
                foreach (Transform child in target)
                {
                    if (!child.TryGetComponent<MaterialBlocker>(out _))
                    {
                        var result = GetMaterialFromRenderer(child, depth - 1);
                        if (result != null)
                        {
                            return result;
                        }
                    }
                }
            }

            return null;
        }

        public static void SetMaterialToGraphic(Transform target, Material material, int depth = 0)
        {
            if (target.TryGetComponent<Graphic>(out var graphic))
            {
                graphic.material = material;
            }
            
            if (depth != 0)
            {
                foreach (Transform child in target)
                {
                    if (!child.TryGetComponent<MaterialBlocker>(out _))
                    {
                        SetMaterialToGraphic(child, material, depth - 1);
                    }
                }
            }
        }

        public static void SetMaterialToRenderer(Transform target, Material material, int index = 0, int depth = 0)
        {
            if (target.TryGetComponent<Renderer>(out var renderer))
            {
                var materials = renderer.sharedMaterials;
                materials[index] = material;
                renderer.sharedMaterials = materials;
            }

            if (depth != 0)
            {
                foreach (Transform child in target)
                {
                    if (!child.TryGetComponent<MaterialBlocker>(out _))
                    {
                        SetMaterialToRenderer(child, material, index, depth - 1);
                    }
                }
            }
        }
    }
}