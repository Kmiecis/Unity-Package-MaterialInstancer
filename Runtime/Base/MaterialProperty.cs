using System.Collections.Generic;
using UnityEngine;

namespace Common.Materials
{
    [ExecuteAlways]
    public abstract class MaterialProperty<T> : MonoBehaviour
    {
        [SerializeField] protected MaterialInstance[] _instances = null;

        [SerializeField] protected T _value;

        public T Value
        {
            get => _value;
            set => SetPropertyValue(value);
        }

        public MaterialProperty()
        {
            _value = GetDefaultValue();
        }

        public IEnumerable<MaterialInstance> GetInstances()
        {
            foreach (var instance in _instances)
            {
                if (instance != null)
                {
                    yield return instance;
                }
            }
        }

        protected abstract void ApplyPropertyValue(Material material, T value);

        protected abstract T ReadPropertyValue(Material material);

        protected virtual T GetDefaultValue()
        {
            return default;
        }

        private IEnumerable<Material> GetClones()
        {
            foreach (var instance in GetInstances())
            {
                var clone = instance.Clone;
                if (clone != null)
                {
                    yield return clone;
                }
            }
        }

        private void ApplyPropertyValue()
        {
            SetPropertyValue(_value);
        }

        private void SetPropertyValue(T value)
        {
            foreach (var clone in GetClones())
            {
                ApplyPropertyValue(clone, value);
            }

            _value = value;
        }

        private void RestorePropertyValues()
        {
            foreach (var instance in GetInstances())
            {
                if (instance.Original != null && instance.HasClone)
                {
                    var defaultValue = ReadPropertyValue(instance.Original);
                    ApplyPropertyValue(instance.Clone, defaultValue);
                }
            }
        }

        private void Start()
        {
            ApplyPropertyValue();
        }

        private void OnDidApplyAnimationProperties()
        {
            ApplyPropertyValue();
        }

        private void OnDestroy()
        {
            RestorePropertyValues();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            ApplyPropertyValue();
        }

        private void Reset()
        {
            _instances = transform.GetComponentsInChildren<MaterialInstance>();
        }
#endif
    }
}
