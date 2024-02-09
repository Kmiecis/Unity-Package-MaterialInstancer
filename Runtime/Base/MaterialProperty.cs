using UnityEngine;

namespace Common.Materials
{
    [ExecuteAlways]
    public abstract class MaterialProperty<T> : MonoBehaviour
    {
        [SerializeField] protected MaterialInstance _instance = null;
        [SerializeField] protected bool _active;
        [SerializeField] protected T _value;
        
        private bool _changed;

        public bool IsActive
        {
            get => _active;
            set => SetActive(value);
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

        public MaterialProperty()
        {
            var value = GetDefaultValue();

            SetPropertyValue(value);
        }

        protected abstract void ApplyPropertyValue(Material material, T value);

        protected abstract T ReadPropertyValue(Material material);

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
            else if (_changed)
            {
                RestorePropertyValues();
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

        private void ApplyPropertyValue(T value)
        {
            if (_instance != null &&
                _instance.GetClone(out var clone))
            {
                ApplyPropertyValue(clone, value);
            }

            _changed = true;
        }

        private void RestorePropertyValues()
        {
            if (_instance != null &&
                _instance.TryGetClone(out var clone))
            {
                var defaultValue = ReadPropertyValue(_instance.Original);
                ApplyPropertyValue(clone, defaultValue);
            }

            _changed = false;
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
            RefreshPropertyValue();
        }

        private void Reset()
        {
            _instance = transform.GetComponentInChildren<MaterialInstance>();
        }
#endif
    }
}
