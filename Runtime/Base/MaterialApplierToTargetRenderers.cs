using System.Collections.Generic;
using UnityEngine;

namespace Common.Materials
{
    [AddComponentMenu(nameof(Common) + "/" + nameof(Materials) + "/Material Applier To Target Renderers")]
    public class MaterialApplierToTargetRenderers : MaterialApplierToTarget
    {
        [SerializeField] private int _index = -1;

        private List<Renderer> _renderers;
        private List<Material> _cache;
        private List<Material> _originals;

        public int Index
        {
            get => _index;
            set => SetIndex(value);
        }

        public MaterialApplierToTargetRenderers()
        {
            _renderers = new List<Renderer>();
            _cache = new List<Material>();
            _originals = new List<Material>();
        }

        public void SetIndex(int index)
        {
            Clear();

            _index = index;

            Apply();
        }

        protected override void ApplyMaterial(Transform target, Material material)
        {
            if (target.TryGetComponent<Renderer>(out var renderer))
            {
                renderer.GetSharedMaterials(_cache);

                if (-1 < _index && _index < _cache.Count)
                {
                    _renderers.Add(renderer);
                    _originals.Add(_cache[_index]);

                    _cache[_index] = material;
                }
                else
                {
                    _cache.Add(material);
                }

                renderer.SetSharedMaterials(_cache);
#if UNITY_EDITOR
                UnityEditor.EditorUtility.SetDirty(renderer);
#endif
                _cache.Clear();
            }
        }

        protected override void RemoveMaterial(Transform target, Material material)
        {
            if (target.TryGetComponent<Renderer>(out var renderer))
            {
                renderer.GetSharedMaterials(_cache);

                if (_renderers.TryIndexOf(renderer, out var index))
                {
                    _renderers.RemoveAt(index);
                    var original = _originals.RevokeAt(index);

                    _cache[_index] = original;
                }
                else
                {
                    while (_cache.Remove(material))
                    {
                    }
                }

                renderer.SetSharedMaterials(_cache);
#if UNITY_EDITOR
                UnityEditor.EditorUtility.SetDirty(renderer);
#endif
                _cache.Clear();
            }
        }
    }
}