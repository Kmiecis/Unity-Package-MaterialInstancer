using System.Collections.Generic;
using UnityEngine;

namespace Common.Materials
{
    [ExecuteAlways]
    public abstract class MaterialApplier : MonoBehaviour
    {
        [SerializeField] private MaterialInstance[] _instances;

        public bool HasClone()
        {
            foreach (var instance in GetInstances())
            {
                if (instance.TryGetClone(out _))
                {
                    return true;
                }
            }
            return false;
        }

        public void CreateClone()
        {
            foreach (var instance in GetInstances())
            {
                if (instance.CreateClone(out var material))
                {
                    ApplyMaterial(material);
                }
            }
        }

        public void ApplyClone()
        {
            foreach (var instance in GetInstances())
            {
                if (instance.GetClone(out var material))
                {
                    ApplyMaterial(material);
                }
            }
        }

        public void ReapplyClone()
        {
            foreach (var instance in GetInstances())
            {
                if (instance.TryGetClone(out var material))
                {
                    RemoveMaterial(material);
                    ApplyMaterial(material);
                }
            }
        }

        public void ApplyCurrent()
        {
            foreach (var instance in GetInstances())
            {
                var material = instance.Current;

                ApplyMaterial(material);
            }
        }

        public void RemoveClone()
        {
            foreach (var instance in GetInstances())
            {
                if (instance.TryGetClone(out var material))
                {
                    RemoveMaterial(material);
                }
            }
        }

        public void ClearClone()
        {
            foreach (var instance in GetInstances())
            {
                if (instance.TryGetClone(out var material))
                {
                    RemoveMaterial(material);
                    
                    instance.ClearClone();
                }
            }
        }

        public IEnumerable<MaterialInstance> GetInstances()
        {
            if (_instances != null)
            {
                for (int i = 0; i < _instances.Length; ++i)
                {
                    yield return _instances[i];
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
                CreateClone();
        }

        private void OnDisable()
        {
#if UNITY_EDITOR
            if (Application.isPlaying)
#endif
                ClearClone();
        }

        private void OnDestroy()
        {
            ClearClone();
        }

#if UNITY_EDITOR
        protected virtual void Reset()
        {
            _instances = GetComponentsInChildren<MaterialInstance>();

            if (enabled)
            {
                CreateClone();
            }
        }

        private void OnSelected()
        {
            var selection = UnityEditor.Selection.objects;
            var selected = IsSelected(selection);
            OnSelection(selected);

            if (!selected)
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
            return false;
        }

        private void OnSelection(bool selected)
        {
            if (this && enabled)
            {
                if (selected)
                {
                    CreateClone();
                }
                else
                {
                    ClearClone();
                }
            }
            else
            {
                ClearClone();
            }
        }

        private void OnDrawGizmosSelected()
        {
            UnityEditor.EditorApplication.update -= OnSelected;
            UnityEditor.EditorApplication.update += OnSelected;
        }
#endif
    }
}