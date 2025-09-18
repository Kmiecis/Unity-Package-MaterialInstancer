using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Common.Materials
{
    [ExecuteAlways]
    public abstract class MaterialApplier : MonoBehaviour
    {
        [SerializeField] private List<MaterialInstance> _instances;

        public void AddInstance(MaterialInstance instance)
        {
            void Change()
            {
                _instances.Add(instance);
            }

            ChangeSafely(Change);
        }

        public void AddInstances(IEnumerable<MaterialInstance> instances)
        {
            void Change()
            {
                _instances.AddRange(instances);
            }

            ChangeSafely(Change);
        }

        public void RemoveInstance(MaterialInstance instance)
        {
            void Change()
            {
                _instances.Remove(instance);
            }

            ChangeSafely(Change);
        }

        public void RemoveInstances(IEnumerable<MaterialInstance> instances)
        {
            void Change()
            {
                _instances.RemoveRange(instances);
            }

            ChangeSafely(Change);
        }

        public void ApplyCurrent()
        {
            foreach (var instance in GetInstances())
            {
                var material = instance.Current;

                ApplyMaterial(material);
            }
        }

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
                for (int i = 0; i < _instances.Count; ++i)
                {
                    yield return _instances[i];
                }
            }
        }

        protected void ChangeSafely(Action applier)
        {
            if (HasClone())
            {
                RemoveClone();

                applier();

                ApplyClone();
            }
            else
            {
                applier();
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
            enabled = false;
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

        [ContextMenu("Search Instances")]
        private void SearchInstances()
        {
            var instances = GetComponentsInChildren<MaterialInstance>();
            _instances = new List<MaterialInstance>(instances);

            UnityEditor.EditorUtility.SetDirty(this);
        }
#endif
    }
}