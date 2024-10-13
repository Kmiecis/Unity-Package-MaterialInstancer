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
                _renderers.Add(renderer);

                var materials = new List<Material>();

                renderer.GetSharedMaterials(materials);
                if (-1 < _index && _index < materials.Count)
                {
                    _originals.Add(materials[_index]);

                    materials[_index] = material;
                }
                else
                {
                    _originals.Add(null);

                    materials.Add(material);
                }

                renderer.sharedMaterials = materials.ToArray();
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
            for (int i = _renderers.Count - 1; i > -1; --i)
            {
                var renderer = _renderers.RevokeAt(i);
                var original = _originals.RevokeAt(i);

                var materials = new List<Material>();
                renderer.GetSharedMaterials(materials);

                if (original != null)
                {
                    while (materials.Count <= _index)
                        materials.Add(null);
                    materials[_index] = original;
                }
                else
                {
                    materials.Remove(material);
                }

                renderer.SetSharedMaterials(materials);
            }
        }
    }
}