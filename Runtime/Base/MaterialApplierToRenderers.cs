using System.Collections.Generic;
using UnityEngine;

namespace Common.Materials
{
    [AddComponentMenu(nameof(Common) + "/" + nameof(Materials) + "/Material Applier To Renderers")]
    public class MaterialApplierToRenderers : MaterialApplier
    {
        [SerializeField] private List<Renderer> _renderers;
        [SerializeField] private int _index = -1;

        private List<Material> _cache;
        private List<Material> _originals;

        public MaterialApplierToRenderers()
        {
            _renderers = new List<Renderer>();

            _cache = new List<Material>();
            _originals = new List<Material>();
        }

        public void AddRenderer(Renderer renderer)
        {
            Clear();

            _renderers.Add(renderer);

            Apply();
        }

        public void AddRenderers(IEnumerable<Renderer> renderers)
        {
            Clear();

            _renderers.AddRange(renderers);

            Apply();
        }

        public void RemoveRenderer(Renderer renderer)
        {
            Clear();

            _renderers.Remove(renderer);

            Apply();
        }

        public void RemoveRenderers(IEnumerable<Renderer> renderers)
        {
            Clear();

            _renderers.RemoveRange(renderers);

            Apply();
        }

        public void ClearRenderers()
        {
            Clear();

            _renderers.Clear();

            Apply();
        }

        public void SetIndex(int index)
        {
            Clear();

            _index = index;

            Apply();
        }

        protected override void ApplyMaterial(Material material)
        {
            for (int i = 0; i < _renderers.Count; ++i)
            {
                var renderer = _renderers[i];

                renderer.GetSharedMaterials(_cache);

                if (-1 < _index && _index < _cache.Count)
                {
                    _originals.Add(_cache[_index]);

                    _cache[_index] = material;
                }
                else
                {
                    _originals.Add(null);

                    _cache.Add(material);
                }

                renderer.SetSharedMaterials(_cache);
#if UNITY_EDITOR
                UnityEditor.EditorUtility.SetDirty(renderer);
#endif
                _cache.Clear();
            }
        }

        protected override void RemoveMaterial(Material material)
        {
            var length = Mathf.Min(_renderers.Count, _originals.Count);
            for (int i = length - 1; i > -1; --i)
            {
                var renderer = _renderers[i];

                renderer.GetSharedMaterials(_cache);

                var original = _originals.RevokeAt(i);
                if (original != null)
                {
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

#if UNITY_EDITOR
        protected override void SearchContext()
        {
            base.SearchContext();

            var renderers = GetComponentsInChildren<Renderer>();
            _renderers = new List<Renderer>(renderers);
        }
#endif
    }
}