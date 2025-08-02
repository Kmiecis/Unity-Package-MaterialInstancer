using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Common.Materials
{
    [AddComponentMenu(nameof(Common) + "/" + nameof(Materials) + "/Material Applier To Graphics")]
    public class MaterialApplierToGraphics : MaterialApplier
    {
        [SerializeField] private Graphic[] _graphics;

        private List<Material> _originals;

        public MaterialApplierToGraphics()
        {
            _originals = new List<Material>();
        }

        protected override void ApplyMaterial(Material material)
        {
            for (int i = 0; i < _graphics.Length; ++i)
            {
                var graphic = _graphics[i];

                _originals.Add(graphic.material);

                graphic.material = material;
#if UNITY_EDITOR
                UnityEditor.EditorUtility.SetDirty(graphic);
#endif
            }
        }

        protected override void RemoveMaterial(Material material)
        {
            for (int i = _graphics.Length - 1; i > -1; --i)
            {
                var graphic = _graphics[i];

                var original = _originals.RevokeAt(i);

                graphic.material = original;
#if UNITY_EDITOR
                UnityEditor.EditorUtility.SetDirty(graphic);
#endif
            }
        }

#if UNITY_EDITOR
        protected override void Reset()
        {
            _graphics = GetComponentsInChildren<Graphic>();

            base.Reset();
        }
#endif
    }
}