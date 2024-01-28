using System.Collections.Generic;
using UnityEngine;

namespace Common.Materials
{
    [ExecuteAlways]
    public abstract class MaterialPropertyNamed<T> : MonoBehaviour
    {
        [SerializeField] protected MaterialInstance[] _instances = null;
        [SerializeField] protected bool _active;
        [SerializeField] protected T _value;
        [SerializeField] protected string _name;

        private bool _dirty;
        private int _id;

        public bool IsActive
        {
            get => _active;
            set => SetActive(value);
        }

        public T Value
        {
            get => _value;
            set => SetPropertyValue(value);
        }

        public string Name
        {
            get => _name;
            set => SetPropertyName(value);
        }

        public MaterialPropertyNamed()
        {
            var value = GetDefaultValue();

            SetPropertyValue(value);
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

        protected abstract void ApplyPropertyValue(Material material, int id, T value);

        protected abstract T ReadPropertyValue(Material material, int id);

        protected virtual T GetDefaultValue()
        {
            return default;
        }

        protected void RefreshPropertyValue()
        {
            if (_active)
            {
                ApplyPropertyValue(_value);
            }
            else if (_dirty)
            {
                RestorePropertyValues();
            }
        }

        private IEnumerable<Material> GetClones()
        {
            foreach (var instance in GetInstances())
            {
                if (instance.GetClone(out var clone))
                {
                    yield return clone;
                }
            }
        }

        private void SetActive(bool value)
        {
            _active = value;

            RefreshPropertyValue();
        }

        private void SetPropertyValue(T value)
        {
            _value = value;

            RefreshPropertyValue();
        }

        private void SetPropertyName(string value)
        {
            _name = value;

            RefreshPropertyId();
            RefreshPropertyValue();
        }

        private void ApplyPropertyValue(T value)
        {
            foreach (var clone in GetClones())
            {
                ApplyPropertyValue(clone, _id, value);
            }

            _dirty = true;
        }

        private void RestorePropertyValues()
        {
            foreach (var instance in GetInstances())
            {
                if (instance.TryGetClone(out var clone))
                {
                    var defaultValue = ReadPropertyValue(instance.Original, _id);
                    ApplyPropertyValue(clone, _id, defaultValue);
                }
            }

            _dirty = false;
        }

        private void RefreshPropertyId()
        {
            _id = Shader.PropertyToID(_name);
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
        private void OnValidate()
        {
            RefreshPropertyId();
            RefreshPropertyValue();
        }

        private void Reset()
        {
            _instances = transform.GetComponentsInChildren<MaterialInstance>();
        }
#endif
    }
}
