using UnityEngine;

namespace Common.Materials
{
    public abstract class AMaterialValue<T> : MonoBehaviour
    {
        [SerializeField]
        protected MaterialInstance _instance = null;

        [SerializeField]
        protected string _propertyName;
        [SerializeField]
        protected T _propertyValue;

        protected int _propertyId;
        protected T _propertyCachedValue;

        public Material MaterialOriginal
        {
            get => _instance != null ? _instance.Original : null;
        }

        public Material MaterialCopy
        {
            get => _instance != null ? _instance.Copy : null;
        }

        public string Name
        {
            get => _propertyName;
            set
            {
                _propertyName = value;
                RefreshPropertyId();
            }
        }

        public T Value
        {
            get => _propertyValue;
            set
            {
                ApplyPropertyValue(value);
                _propertyValue = value;
            }
        }

        public int Id
        {
            get => _propertyId;
        }

        protected abstract void ApplyPropertyValue(Material material, int id, T value);

        protected abstract T ReadPropertyValue(Material material, int id);

        private void ApplyPropertyValue(T value)
        {
            ApplyPropertyValue(MaterialCopy, _propertyId, value);
        }

        private T ReadPropertyValue()
        {
            return ReadPropertyValue(MaterialOriginal, _propertyId);
        }

        private bool TryReadPropertyValue(out T value)
        {
            var target = MaterialOriginal;
            if (target != null &&
                target.HasProperty(_propertyId))
            {
                value = ReadPropertyValue(target, _propertyId);
                return true;
            }

            value = default;
            return false;
        }

        private void ApplyPropertyValueIfDirty(T value)
        {
            if (!Equals(_propertyCachedValue, default(T)) &&
                !Equals(_propertyCachedValue, value))
            {
                ApplyPropertyValue(value);
                _propertyCachedValue = value;
            }
        }

        private void TryCachePropertyValue()
        {
            if (TryReadPropertyValue(out var originalValue))
            {
                _propertyCachedValue = originalValue;
            }
        }

        private void RefreshPropertyId()
        {
            _propertyId = Shader.PropertyToID(_propertyName);
        }

        private void Start()
        {
            RefreshPropertyId();
            TryCachePropertyValue();

            ApplyPropertyValueIfDirty(_propertyValue);
        }

        private void OnDidApplyAnimationProperties()
        {
            ApplyPropertyValueIfDirty(_propertyValue);
        }

#if UNITY_EDITOR
        protected void OnValidate()
        {
            RefreshPropertyId();
            TryCachePropertyValue();

            ApplyPropertyValueIfDirty(_propertyValue);
        }

        protected virtual void Reset()
        {
            if (_instance == null)
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
