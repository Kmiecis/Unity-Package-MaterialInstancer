using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Common.Materials
{
    [AddComponentMenu(nameof(Common) + "/" + nameof(Materials) + "/Material Applier To Target Graphics")]
    public class MaterialApplierToTargetGraphics : MaterialApplierToTarget
    {
        private List<Graphic> _graphics;
        private List<Material> _originals;

        public MaterialApplierToTargetGraphics()
        {
            _graphics = new List<Graphic>();
            _originals = new List<Material>();
        }

        protected override void ApplyMaterial(Transform target, Material material)
        {
            if (target.TryGetComponent<Graphic>(out var graphic))
            {
                _graphics.Add(graphic);
                _originals.Add(graphic.material);

                graphic.material = material;
#if UNITY_EDITOR
                UnityEditor.EditorUtility.SetDirty(graphic);
#endif
            }
        }

        protected override void RemoveMaterial(Transform target, Material material)
        {
            if (target.TryGetComponent<Graphic>(out var graphic))
            {
                if (_graphics.TryIndexOf(graphic, out var index))
                {
                    _graphics.RemoveAt(index);
                    var original = _originals.RevokeAt(index);

                    graphic.material = original;
#if UNITY_EDITOR
                    UnityEditor.EditorUtility.SetDirty(graphic);
#endif
                }
            }
        }
    }
}