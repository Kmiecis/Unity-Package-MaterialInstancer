using UnityEngine;

namespace Common.Materials
{
    [ExecuteAlways]
    public abstract class MaterialPropertyNamed<T> : MonoBehaviour
    {
        [SerializeField] protected MaterialInstance _instance = null;
        [SerializeField] protected bool _active;
        [SerializeField] protected string _name;
        [SerializeField] protected T _value;

        private bool _changed;
        private int _id;

        public bool IsActive
        {
            get => _active;
            set => SetActive(value);
        }

        public string Name
        {
            get => _name;
            set => SetPropertyName(value);
        }

        public T Value
        {
            get => GetValue();
            set => SetValue(value);
        }

        public T GetValue()
        {
            return _value;
        }

        public void SetValue(T value)
        {
            SetPropertyValue(value);
        }

        public MaterialPropertyNamed()
        {
            var value = GetDefaultValue();

            SetPropertyValue(value);
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

        private void SetPropertyName(string value)
        {
            _name = value;

            RefreshPropertyId();
            RefreshPropertyValue();
        }

        private void ApplyPropertyValue(T value)
        {
            if (_instance != null &&
                _instance.GetClone(out var clone))
            {
                ApplyPropertyValue(clone, _id, value);
            }

            _changed = true;
        }

        private void RestorePropertyValues()
        {
            if (_instance != null &&
                _instance.TryGetClone(out var clone))
            {
                var defaultValue = ReadPropertyValue(_instance.Original, _id);
                ApplyPropertyValue(clone, _id, defaultValue);
            }

            _changed = false;
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
            _instance = transform.GetComponentInChildren<MaterialInstance>();
        }
#endif
    }
}
