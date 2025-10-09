using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Common.Materials
{
    [AddComponentMenu(nameof(Common) + "/" + nameof(Materials) + "/Material Applier To Graphics")]
    public class MaterialApplierToGraphics : MaterialApplier
    {
        [SerializeField] private List<Graphic> _graphics;

        private List<Material> _originals;

        public MaterialApplierToGraphics()
        {
            _graphics = new List<Graphic>();

            _originals = new List<Material>();
        }

        public void AddGraphic(Graphic graphic)
        {
            Clear();

            _graphics.Add(graphic);

            Apply();
        }

        public void AddGraphics(IEnumerable<Graphic> graphics)
        {
            Clear();

            _graphics.AddRange(graphics);

            Apply();
        }

        public void RemoveGraphic(Graphic graphic)
        {
            Clear();

            _graphics.Remove(graphic);

            Apply();
        }

        public void RemoveGraphics(IEnumerable<Graphic> graphics)
        {
            Clear();

            _graphics.RemoveRange(graphics);

            Apply();
        }

        public void ClearGraphics()
        {
            Clear();

            _graphics.Clear();

            Apply();
        }

        protected override void ApplyMaterial(Material material)
        {
            for (int i = 0; i < _graphics.Count; ++i)
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
            for (int i = _graphics.Count - 1; i > -1; --i)
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
        protected override void SearchContext()
        {
            base.SearchContext();

            var graphics = GetComponentsInChildren<Graphic>();
            _graphics = new List<Graphic>(graphics);
        }
#endif
    }
}