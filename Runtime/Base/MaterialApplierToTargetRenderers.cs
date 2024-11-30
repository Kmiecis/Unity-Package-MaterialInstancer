using System.Collections.Generic;
using UnityEngine;

namespace Common.Materials
{
    [AddComponentMenu(nameof(Common) + "/" + nameof(Materials) + "/" + "Material Applier To Target Renderers")]
    public class MaterialApplierToTargetRenderers : MaterialApplierToTarget
    {
        [SerializeField] private int _index;

        private List<Renderer> _renderers;
        private List<Material> _originals;

        public int Index
        {
            get => _index;
            set => SetIndex(value);
        }

        public MaterialApplierToTargetRenderers()
        {
            _renderers = new List<Renderer>();
            _originals = new List<Material>();
        }

        protected override void ApplyMaterial(Transform target, Material material)
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
        }

        protected override void RemoveMaterial(Transform target, Material material)
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
                    while (materials.Remove(material))
                    {
                    }
                }

                renderer.SetSharedMaterials(materials);
            }
        }

        private void SetIndex(int index)
        {
            if (HasClone())
            {
                ClearClone();

                _index = index;

                ApplyClone();
            }
        }
    }
}