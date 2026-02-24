using System.Collections.Generic;
using UnityEngine;

namespace Common.Materials
{
    [AddComponentMenu(nameof(Common) + "/" + nameof(Materials) + "/Material Applier To Target Renderers")]
    public class MaterialApplierToTargetRenderers : MaterialApplierToTarget
    {
        [SerializeField] private int _index = -1;

        private List<Renderer> _renderers;
        private List<Material> _temp;
        private List<Material> _originals;

        public int Index
        {
            get => _index;
            set => SetIndex(value);
        }

        public MaterialApplierToTargetRenderers()
        {
            _renderers = new List<Renderer>();
            _temp = new List<Material>();
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
                renderer.GetSharedMaterials(_temp);

                if (-1 < _index && _index < _temp.Count)
                {
                    _renderers.Add(renderer);
                    _originals.Add(_temp[_index]);

                    _temp[_index] = material;
                }
                else
                {
                    _temp.Add(material);
                }

                renderer.SetSharedMaterials(_temp);
#if UNITY_EDITOR
                UnityEditor.EditorUtility.SetDirty(renderer);
#endif
                _temp.Clear();
            }
        }

        protected override void RemoveMaterial(Transform target, Material material)
        {
            if (target.TryGetComponent<Renderer>(out var renderer))
            {
                renderer.GetSharedMaterials(_temp);

                if (_renderers.TryIndexOf(renderer, out var index))
                {
                    _renderers.RemoveAt(index);
                    var original = _originals.RevokeAt(index);

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
    }
}