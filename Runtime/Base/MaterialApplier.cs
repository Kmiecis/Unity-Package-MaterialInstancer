using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Common.Materials
{
    [ExecuteAlways]
    public abstract class MaterialApplier : MonoBehaviour
    {
        [SerializeField] private List<MaterialReference> _references;

        public void AddReference(MaterialReference reference)
        {
            Clear();

            _references.Add(reference);

            Apply();
        }

        public void AddReferences(IEnumerable<MaterialReference> references)
        {
            Clear();

            _references.AddRange(references);

            Apply();
        }

        public void RemoveReference(MaterialReference reference)
        {
            Clear();

            _references.Remove(reference);

            Apply();
        }

        public void RemoveReferences(IEnumerable<MaterialReference> references)
        {
            Clear();

            _references.RemoveRange(references);

            Apply();
        }

        public void Apply()
        {
            foreach (var reference in GetReferences())
            {
                var material = reference.Material;

                ApplyMaterial(material);
            }
        }

        public void Clear()
        {
            foreach (var reference in GetReferences())
            {
                var material = reference.Material;

                RemoveMaterial(material);
            }
        }

        public void Reapply()
        {
            Clear();
            Apply();
        }

        public IEnumerable<MaterialReference> GetReferences()
        {
            if (_references != null)
            {
                for (int i = 0; i < _references.Count; ++i)
                {
                    yield return _references[i];
                }
            }
        }

        protected abstract void ApplyMaterial(Material material);

        protected abstract void RemoveMaterial(Material material);

        private void OnEnable()
        {
#if UNITY_EDITOR
            if (Application.isPlaying)
#endif
                Apply();
        }

        private void OnDisable()
        {
#if UNITY_EDITOR
            if (Application.isPlaying)
#endif
                Clear();
        }

        private void OnDestroy()
        {
            Clear();
        }

#if UNITY_EDITOR
        [ContextMenu("Search References")]
        private void SearchReferences()
        {
            var references = GetComponentsInChildren<MaterialReference>();
            _references = new List<MaterialReference>(references);

            UnityEditor.EditorUtility.SetDirty(this);
        }

        private void OnSelected()
        {
            try
            {
                var selection = UnityEditor.Selection.objects;
                var selected = IsSelected(selection);
                OnSelection(selected);
            }
            catch
            {
                UnityEditor.EditorApplication.update -= OnSelected;
            }
        }

        private bool IsSelected(Object[] objects)
        {
            foreach (var item in objects)
            {
                if (item == gameObject)
                {
                    return true;
                }
            }
            throw new System.Exception();
        }

        private bool _selected;

        private void OnSelection(bool selected)
        {
            if (this && enabled)
            {
                if (selected && !_selected)
                {
                    Apply();

                    _selected = true;
                }
                else if (!selected && _selected)
                {
                    Clear();

                    _selected = false;
                }
            }
            else if (_selected)
            {
                Clear();

                _selected = false;
            }
        }

        private void OnDrawGizmosSelected()
        {
            UnityEditor.EditorApplication.update -= OnSelected;
            UnityEditor.EditorApplication.update += OnSelected;
        }

        protected virtual void Reset()
        {
            enabled = false;

            SearchReferences();
        }
#endif
    }
}