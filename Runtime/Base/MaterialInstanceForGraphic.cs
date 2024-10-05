using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Common.Materials
{
    [AddComponentMenu(nameof(Common) + "/" + nameof(Materials) + "/" + nameof(MaterialInstanceForGraphic))]
    public class MaterialInstanceForGraphic : MaterialInstance
    {
        private readonly List<Graphic> _graphics;
        private readonly List<Material> _originals;

        public MaterialInstanceForGraphic()
        {
            _graphics = new List<Graphic>();
            _originals = new List<Material>();
        }

        protected override void ApplyMaterial(Transform target, Material material, int depth)
        {
            if (target.TryGetComponent<Graphic>(out var graphic))
            {
                _graphics.Add(graphic);
                _originals.Add(graphic.material);

                graphic.material = material;
            }

            if (depth != 0)
            {
                foreach (Transform child in target)
                {
                    if (!child.TryGetComponent<MaterialBlocker>(out _))
                    {
                        ApplyMaterial(child, material, depth - 1);
                    }
                }
            }
        }

        protected override void RemoveMaterial(Material material, int depth)
        {
            for (int i = _graphics.Count - 1; i > -1; --i)
            {
                var graphic = _graphics.RevokeAt(i);
                var original = _originals.RevokeAt(i);

                graphic.material = original;
            }
        }
    }
}