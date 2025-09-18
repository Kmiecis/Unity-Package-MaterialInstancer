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
            void Applier()
            {
                _graphics.Add(graphic);
            }

            ChangeSafely(Applier);
        }

        public void AddGraphics(IEnumerable<Graphic> graphics)
        {
            void Applier()
            {
                _graphics.AddRange(graphics);
            }

            ChangeSafely(Applier);
        }

        public void RemoveGraphic(Graphic graphic)
        {
            void Applier()
            {
                _graphics.Remove(graphic);
            }

            ChangeSafely(Applier);
        }

        public void RemoveGraphics(IEnumerable<Graphic> graphics)
        {
            void Applier()
            {
                _graphics.RemoveRange(graphics);
            }

            ChangeSafely(Applier);
        }

        public void ClearGraphics()
        {
            void Applier()
            {
                _graphics.Clear();
            }

            ChangeSafely(Applier);
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
        [ContextMenu("Search Graphics")]
        private void SearchInstances()
        {
            var graphics = GetComponentsInChildren<Graphic>();
            _graphics = new List<Graphic>(graphics);

            UnityEditor.EditorUtility.SetDirty(this);
        }
#endif
    }
}