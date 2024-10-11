using System.Collections.Generic;
using UnityEngine;

namespace Common.Materials
{
    [ExecuteAlways]
    public abstract class MaterialPropertyBase<T> : MonoBehaviour
    {
        [SerializeField] protected MaterialInstance[] _instances;
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

        public MaterialPropertyBase()
        {
            var value = GetDefaultValue();

            SetPropertyValue(value);
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
            if (_active)
            {
                ApplyPropertyValue(_value);

                _changed = true;
            }
            else if (_changed)
            {
                RestorePropertyValues();

                _changed = false;
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

        protected abstract void ApplyPropertyValue(T value);

        protected abstract void RestorePropertyValues();

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
        }
#endif
    }
}
