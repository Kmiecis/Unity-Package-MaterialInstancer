using System.Collections.Generic;
using UnityEngine;

namespace Common.Materials
{
    [AddComponentMenu(nameof(Common) + "/" + nameof(Materials) + "/Material Applier To Renderers")]
    public class MaterialApplierToRenderers : MaterialApplier
    {
        [SerializeField] private List<Renderer> _renderers;
        [SerializeField] private int _index = -1;

        private List<Material> _temp;
        private List<Material> _originals;

        public MaterialApplierToRenderers()
        {
            _renderers = new List<Renderer>();

            _temp = new List<Material>();
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
            while (_originals.Count < _renderers.Count)
            {
                var renderer = _renderers[_originals.Count];

                renderer.GetSharedMaterials(_temp);

                if (-1 < _index && _index < _temp.Count)
                {
                    _originals.Add(_temp[_index]);

                    _temp[_index] = material;
                }
                else
                {
                    _originals.Add(null);

                    _temp.Add(material);
                }

                renderer.SetSharedMaterials(_temp);
#if UNITY_EDITOR
                UnityEditor.EditorUtility.SetDirty(renderer);
#endif
                _temp.Clear();
            }
        }

        protected override void RemoveMaterial(Material material)
        {
            var length = Mathf.Min(_renderers.Count, _originals.Count);
            for (int i = length - 1; i > -1; --i)
            {
                var renderer = _renderers[i];

                renderer.GetSharedMaterials(_temp);

                var original = _originals.RevokeAt(i);
                if (original != null)
                {
                    _temp[_index] = original;
                }
                else
                {
                    while (_temp.Remove(material))
                    {
                    }
                }

                renderer.SetSharedMaterials(_temp);
#if UNITY_EDITOR
                UnityEditor.EditorUtility.SetDirty(renderer);
#endif
                _temp.Clear();
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