using System.Collections.Generic;
using UnityEngine;

namespace Common.Materials
{
    [ExecuteAlways]
    public abstract class MaterialPropertyNamed<T> : MonoBehaviour
    {
        [SerializeField] protected MaterialInstance[] _instances = null;

        [SerializeField] protected string _name;
        [SerializeField] protected T _value;

        protected int _id;

        public string Name
        {
            get => _name;
            set => SetPropertyName(value);
        }

        public T Value
        {
            get => _value;
            set => SetPropertyValue(value);
        }

        public int Id
        {
            get => _id;
        }

        public MaterialPropertyNamed()
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

        protected abstract void ApplyPropertyValue(Material material, int id, T value);

        protected abstract T ReadPropertyValue(Material material, int id);

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

        private void SetPropertyName(string value)
        {
            _name = value;

            RefreshPropertyId();
        }

        private void SetPropertyValue(T value)
        {
            foreach (var clone in GetClones())
            {
                ApplyPropertyValue(clone, _id, value);
            }

            _value = value;
        }

        private void RestorePropertyValues()
        {
            foreach (var instance in GetInstances())
            {
                if (instance.Original != null && instance.HasClone)
                {
                    var defaultValue = ReadPropertyValue(instance.Original, _id);
                    ApplyPropertyValue(instance.Clone, _id, defaultValue);
                }
            }
        }

        private void RefreshPropertyId()
        {
            _id = Shader.PropertyToID(_name);
        }

        private void Start()
        {
            RefreshPropertyId();
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
            RefreshPropertyId();
            ApplyPropertyValue();
        }

        private void Reset()
        {
            _instances = transform.GetComponentsInChildren<MaterialInstance>();
        }
#endif
    }
}
