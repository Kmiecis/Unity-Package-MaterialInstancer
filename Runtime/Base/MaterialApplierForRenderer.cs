using System.Collections.Generic;
using UnityEngine;

namespace Common.Materials
{
    [AddComponentMenu(nameof(Common) + "/" + nameof(Materials) + "/" + nameof(MaterialApplierForRenderer))]
    public class MaterialApplierForRenderer : MaterialApplier
    {
        [SerializeField] private int _index;

        private List<Renderer> _renderers;
        private List<Material> _originals;

        public MaterialApplierForRenderer()
        {
            _renderers = new List<Renderer>();
            _originals = new List<Material>();
        }

        protected override void ApplyMaterial(Transform target, Material material, int depth)
        {
            if (target.TryGetComponent<Renderer>(out var renderer))
            {
                var materials = new List<Material>();
                renderer.GetSharedMaterials(materials);

                if (-1 < _index && _index < materials.Count)
                {
                    _renderers.Add(renderer);
                    _originals.Add(materials[_index]);

                    materials[_index] = material;
                }
                else
                {
                    materials.Add(material);
                }

                renderer.SetSharedMaterials(materials);
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

        protected override void RemoveMaterial(Transform target, Material material, int depth)
        {
            if (target.TryGetComponent<Renderer>(out var renderer))
            {
                var materials = new List<Material>();
                renderer.GetSharedMaterials(materials);

                if (_renderers.TryIndexOf(renderer, out var index))
                {
                    _renderers.RemoveAt(index);
                    var original = _originals.RevokeAt(index);

                    materials[_index] = original;
                }
                else
                {
                    materials.Remove(material);
                }

                renderer.SetSharedMaterials(materials);
            }

            if (depth != 0)
            {
                foreach (Transform child in target)
                {
                    if (!child.TryGetComponent<MaterialBlocker>(out _))
                    {
                        RemoveMaterial(child, material, depth - 1);
                    }
                }
            }
        }
    }
}