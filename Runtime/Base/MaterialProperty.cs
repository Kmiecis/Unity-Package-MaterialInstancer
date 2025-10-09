using System.Collections.Generic;
using UnityEngine;

namespace Common.Materials
{
    [ExecuteAlways]
    public abstract class MaterialProperty<T> : MonoBehaviour
    {
        [SerializeField] protected List<MaterialInstance> _instances;
        [SerializeField] protected T _value;

        private bool _changed;

        public void AddInstance(MaterialInstance instance)
        {
            _instances.Add(instance);

            if (_changed)
            {
                ApplyPropertyValue(instance.Material);
            }
        }

        public void AddInstances(IEnumerable<MaterialInstance> instances)
        {
            foreach (var instance in instances)
            {
                AddInstance(instance);
            }
        }

        public void RemoveInstance(MaterialInstance instance)
        {
            if (_instances.Remove(instance) && _changed)
            {
                RestorePropertyValue(instance.Material);
            }
        }

        public void RemoveInstances(IEnumerable<MaterialInstance> instances)
        {
            foreach (var instance in instances)
            {
                RemoveInstance(instance);
            }
        }

        public T GetValue()
        {
            return _value;
        }

        public void SetValue(T value)
        {
            SetPropertyValue(value);
        }

        public T Value
        {
            get => GetValue();
            set => SetValue(value);
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

        protected virtual T GetDefaultValue()
        {
            return default;
        }

        protected void RefreshPropertyValue()
        {
            if (enabled)
            {
                ApplyPropertyValues();

                _changed = true;
            }
            else if (_changed)
            {
                RestorePropertyValues();

                _changed = false;
            }
        }

        private void SetPropertyValue(T value)
        {
            _value = value;

            RefreshPropertyValue();
        }

        protected abstract void ApplyPropertyValue(Material material, T value);

        protected abstract T ReadPropertyValue(Material material);

        private void ApplyPropertyValue(Material material)
        {
            ApplyPropertyValue(material, _value);
        }

        private void RestorePropertyValue(Material material)
        {
            var defaultValue = ReadPropertyValue(material);
            ApplyPropertyValue(material, defaultValue);
        }

        private void ApplyPropertyValues()
        {
            foreach (var instance in GetInstances())
            {
                var material = instance.Material;

                ApplyPropertyValue(material);
            }
        }

        private void RestorePropertyValues()
        {
            foreach (var instance in GetInstances())
            {
                var material = instance.Material;

                RestorePropertyValue(material);
            }
        }

        private void OnEnable()
        {
            RefreshPropertyValue();
        }

        private void OnDisable()
        {
            RefreshPropertyValue();
        }

        private void Start()
        {
            RefreshPropertyValue();
        }

        private void OnDidApplyAnimationProperties()
        {
            RefreshPropertyValue();
        }

        private void OnDestroy()
        {
            RestorePropertyValues();
        }

#if UNITY_EDITOR
        protected virtual void SearchContext()
        {
            var instances = transform.GetComponentsInChildren<MaterialInstance>();
            _instances = new List<MaterialInstance>(instances);
        }

        [ContextMenu("Search Parameters")]
        private void SearchParameters()
        {
            SearchContext();

            UnityEditor.EditorUtility.SetDirty(this);
        }

        protected virtual void OnValidate()
        {
            RefreshPropertyValue();
        }

        private void Reset()
        {
            enabled = false;

            SearchParameters();

            _value = GetDefaultValue();
        }
#endif
    }
}
