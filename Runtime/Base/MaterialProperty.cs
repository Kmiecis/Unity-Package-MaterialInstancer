using System.Collections.Generic;
using UnityEngine;

namespace Common.Materials
{
    [ExecuteAlways]
    public abstract class MaterialProperty<T> : MonoBehaviour
    {
        [SerializeField] protected MaterialInstance[] _instances;
        [SerializeField] protected T _value;

        private bool _changed;

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
                for (int i = 0; i < _instances.Length; ++i)
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

        private void ApplyPropertyValues()
        {
            foreach (var instance in GetInstances())
            {
                if (instance.TryGetClone(out var material))
                {
                    ApplyPropertyValue(material, _value);
                }
            }
        }

        private void RestorePropertyValues()
        {
            foreach (var instance in GetInstances())
            {
                if (instance.TryGetClone(out var material))
                {
                    var defaultValue = ReadPropertyValue(instance.Source);
                    ApplyPropertyValue(material, defaultValue);
                }
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
        protected virtual void OnValidate()
        {
            RefreshPropertyValue();
        }

        private void Reset()
        {
            _instances = transform.GetComponentsInChildren<MaterialInstance>();

            var value = GetDefaultValue();
            SetPropertyValue(value);
        }
#endif
    }
}
