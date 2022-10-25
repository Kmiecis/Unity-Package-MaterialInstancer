using UnityEngine;

namespace Common.Materializer
{
    [ExecuteAlways]
    public abstract class AMaterialValue<T> : MonoBehaviour
    {
        [SerializeField]
        protected MaterialInstance _instance = null;

        [SerializeField]
        protected string _propertyName;
        [SerializeField]
        protected T _propertyValue;

        protected int _propertyId;

        public Material Material
        {
            get => _instance is not null ? _instance.Copy : null;
        }

        public string PropertyName
        {
            get => _propertyName;
            set
            {
                _propertyName = value;
                RefreshPropertyId();
            }
        }

        public T PropertyValue
        {
            get => _propertyValue;
            set
            {
                ApplyPropertyValue(value);
                _propertyValue = value;
            }
        }

        public int PropertyId
        {
            get => _propertyId;
        }

        protected abstract void ApplyPropertyValue(Material material, int id, T value);

        protected abstract T ReadPropertyValue(Material material, int id);

        private void ApplyPropertyValue(T value)
        {
            ApplyPropertyValue(Material, _propertyId, value);
        }

        private T ReadPropertyValue()
        {
            return ReadPropertyValue(Material, _propertyId);
        }

        private void RefreshPropertyId()
        {
            _propertyId = Shader.PropertyToID(_propertyName);
        }

        private void Start()
        {
            RefreshPropertyId();

            var propertyValue = ReadPropertyValue();
            if (!Equals(propertyValue, _propertyValue))
            {
                ApplyPropertyValue(_propertyValue);
            }
        }

        private void OnDidApplyAnimationProperties()
        {
            ApplyPropertyValue(_propertyValue);
        }

#if UNITY_EDITOR
        protected void OnValidate()
        {
            RefreshPropertyId();

            if (Material is not null)
            {
                ApplyPropertyValue(_propertyValue);
            }
        }

        protected virtual void Reset()
        {
            if (_instance is null)
            {
                if (
                    transform.TryGetComponentInParent<MaterialInstance>(out var instance) ||
                    transform.TryGetComponentInChildren<MaterialInstance>(out instance)
                )
                {
                    _instance = instance;
                }
            }
        }
#endif
    }
}
