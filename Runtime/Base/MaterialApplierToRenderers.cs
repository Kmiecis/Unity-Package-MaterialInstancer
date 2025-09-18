using System.Collections.Generic;
using UnityEngine;

namespace Common.Materials
{
    [AddComponentMenu(nameof(Common) + "/" + nameof(Materials) + "/Material Applier To Renderers")]
    public class MaterialApplierToRenderers : MaterialApplier
    {
        [SerializeField] private List<Renderer> _renderers;
        [SerializeField] private int _index = -1;

        private readonly List<Material> _originals;

        public MaterialApplierToRenderers()
        {
            _renderers = new List<Renderer>();

            _originals = new List<Material>();
        }

        public void AddRenderer(Renderer renderer)
        {
            void Applier()
            {
                _renderers.Add(renderer);
            }

            ChangeSafely(Applier);
        }

        public void AddRenderers(IEnumerable<Renderer> renderers)
        {
            void Applier()
            {
                _renderers.AddRange(renderers);
            }

            ChangeSafely(Applier);
        }

        public void RemoveRenderer(Renderer renderer)
        {
            void Applier()
            {
                _renderers.Remove(renderer);
            }

            ChangeSafely(Applier);
        }

        public void RemoveRenderers(IEnumerable<Renderer> renderers)
        {
            void Applier()
            {
                _renderers.RemoveRange(renderers);
            }

            ChangeSafely(Applier);
        }

        public void ClearRenderers()
        {
            void Applier()
            {
                _renderers.Clear();
            }

            ChangeSafely(Applier);
        }

        public void SetIndex(int index)
        {
            void Applier()
            {
                _index = index;
            }

            ChangeSafely(Applier);
        }

        protected override void ApplyMaterial(Material material)
        {
            for (int i = 0; i < _renderers.Count; ++i)
            {
                var renderer = _renderers[i];

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

                renderer.SetSharedMaterials(materials);
#if UNITY_EDITOR
                UnityEditor.EditorUtility.SetDirty(renderer);
#endif
            }
        }

        protected override void RemoveMaterial(Material material)
        {
            var length = Mathf.Min(_renderers.Count, _originals.Count);
            for (int i = length - 1; i > -1; --i)
            {
                var renderer = _renderers[i];

                var materials = new List<Material>();
                renderer.GetSharedMaterials(materials);

                var original = _originals.RevokeAt(i);
                if (original != null)
                {
                    materials[_index] = original;
                }
                else
                {
                    while (materials.Remove(material))
                    {
                    }
                }

                renderer.SetSharedMaterials(materials);
#if UNITY_EDITOR
                UnityEditor.EditorUtility.SetDirty(renderer);
#endif
            }
        }

#if UNITY_EDITOR
        [ContextMenu("Search Renderers")]
        private void SearchInstances()
        {
            var renderers = GetComponentsInChildren<Renderer>();
            _renderers = new List<Renderer>(renderers);

            UnityEditor.EditorUtility.SetDirty(this);
        }
#endif
    }
}