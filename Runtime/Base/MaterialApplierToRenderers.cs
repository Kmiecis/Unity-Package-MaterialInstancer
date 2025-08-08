using System.Collections.Generic;
using UnityEngine;

namespace Common.Materials
{
    [AddComponentMenu(nameof(Common) + "/" + nameof(Materials) + "/Material Applier To Renderers")]
    public class MaterialApplierToRenderers : MaterialApplier
    {
        [SerializeField] private Renderer[] _renderers;
        [SerializeField] private int _index = -1;

        private readonly List<Material> _originals;

        public MaterialApplierToRenderers()
        {
            _originals = new List<Material>();
        }

        protected override void ApplyMaterial(Material material)
        {
            for (int i = 0; i < _renderers.Length; ++i)
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
            var length = Mathf.Min(_renderers.Length, _originals.Count);
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
            _renderers = GetComponentsInChildren<Renderer>();

            UnityEditor.EditorUtility.SetDirty(this);
        }
#endif
    }
}