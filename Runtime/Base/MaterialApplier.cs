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
            Apply();
        }

        private void OnDisable()
        {
            Clear();
        }

#if UNITY_EDITOR
        protected virtual void SearchContext()
        {
            var references = GetComponentsInChildren<MaterialReference>();
            _references = new List<MaterialReference>(references);
        }

        [ContextMenu("Search Parameters")]
        private void SearchParameters()
        {
            SearchContext();

            UnityEditor.EditorUtility.SetDirty(this);
        }

        protected virtual void Reset()
        {
            enabled = false;

            SearchParameters();
        }
#endif
    }
}