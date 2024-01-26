using System;
using System.Collections.Generic;
using UnityEngine;

namespace Common.Materials
{
    public interface IPropertiesInstance
    {
        Color GetColor(int id);

        void SetColor(int id, Color value);

        void ClearColor(int id);

        float GetFloat(int id);
        
        void SetFloat(int id, float value);

        void ClearFloat(int id);
    }

    public abstract class NewMaterialProperty<T> : MonoBehaviour
    {
        [SerializeField] private NewPropertiesInstance[] _instances;
        
        [SerializeField] private string _name;
        [SerializeField] private T _value;

        private int _id;

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

        public NewMaterialProperty()
        {
            _value = GetDefaultValue();
        }

        public IEnumerable<NewPropertiesInstance> GetInstances()
        {
            foreach (var instance in _instances)
            {
                if (instance != null)
                {
                    yield return instance;
                }
            }
        }

        protected void SetPropertyName(string value)
        {
            _name = value;

            RefreshPropertyId();
        }

        protected void SetPropertyValue(T value)
        {
            foreach (var instance in GetInstances())
            {
                ApplyPropertyValue(instance, _id, value);
            }

            _value = value;
        }

        private void ApplyPropertyValue()
        {
            SetPropertyValue(_value);
        }

        protected abstract void ApplyPropertyValue(IPropertiesInstance instance, int id, T value);

        protected abstract T ReadPropertyValue(IPropertiesInstance instance, int id);

        protected virtual T GetDefaultValue()
        {
            return default;
        }

        private void RefreshPropertyId()
        {
            _id = Shader.PropertyToID(_name);
        }

        #region Unity methods

        private void Start()
        {
            RefreshPropertyId();
            ApplyPropertyValue();
        }

        private void OnDidApplyAnimationProperties()
        {
            ApplyPropertyValue();
        }

#if UNITY_EDITOR
        protected void OnValidate()
        {
            RefreshPropertyId();
            ApplyPropertyValue();
        }

        protected virtual void Reset()
        {
            _instances = transform.GetComponentsInChildren<NewPropertiesInstance>();
        }
#endif

        #endregion
    }

    public abstract class NewPropertiesInstance : MonoBehaviour, IPropertiesInstance
    {
        private readonly Dictionary<int, object> _rawValues;
        private readonly Dictionary<int, object> _newValues;

        public NewPropertiesInstance()
        {
            _rawValues = new Dictionary<int, object>();
            _newValues = new Dictionary<int, object>();
        }

        public Color GetColor(int id)
            => GetValue(id, GetRawColor);

        public void SetColor(int id, Color value)
            => SetValue(id, value, GetRawColor, SetRawColor);

        public void ClearColor(int id)
            => ClearValue(id, GetRawColor, SetRawColor);

        public float GetFloat(int id)
            => GetValue(id, GetRawFloat);

        public void SetFloat(int id, float value)
            => SetValue(id, value, GetRawFloat, SetRawFloat);

        public void ClearFloat(int id)
            => ClearValue(id, GetRawFloat, SetRawFloat);

        protected abstract Color GetRawColor(int id);

        protected abstract void SetRawColor(int id, Color value);

        protected abstract float GetRawFloat(int id);

        protected abstract void SetRawFloat(int id, float value);

        private T GetCachedValue<T>(int id, Func<int, T> rawGetter)
        {
            if (!_rawValues.TryGetValue(id, out var rawValue))
            {
                _rawValues[id] = rawValue = rawGetter(id);
            }
            return (T)rawValue;
        }

        private T GetValue<T>(int id, Func<int, T> rawGetter)
        {
            if (!_newValues.TryGetValue(id, out var newValue))
            {
                return GetCachedValue(id, rawGetter);
            }
            return (T)newValue;
        }

        private void SetValue<T>(int id, T value, Func<int, T> rawGetter, Action<int, T> rawSetter)
        {
            SetNewValue(id, value, rawSetter);
        }

        private void SetNewValue<T>(int id, T value, Action<int, T> rawSetter)
        {
            _newValues[id] = value;

            rawSetter(id, value);
        }

        private void ClearValue<T>(int id, Func<int, T> rawGetter, Action<int, T> rawSetter)
        {
            var rawValue = GetCachedValue(id, rawGetter);

            SetNewValue(id, rawValue, rawSetter);
        }
    }

    public abstract class NewMaterialInstance : NewPropertiesInstance
    {
        [SerializeField] private Material _original;
        [SerializeField] private int _depth;

        private Material _copy;

        public Material Original
        {
            get => _original;
            set
            {
                DestroyCopy();

                _original = value;

                ApplyMaterial(_original);
            }
        }

        public Material Copy
        {
            get
            {
                if (_copy == null && _original != null)
                {
                    _copy = new Material(_original);
                    _copy.name += " (Clone)";

                    ApplyMaterial(_copy);
                }
                return _copy;
            }
        }

        public Material Current
        {
            get => _copy != null ? _copy : _original;
        }

        public void Apply()
        {
            ApplyMaterial(Current);
        }

        protected abstract Material ReadMaterial(Transform target, int depth);

        protected abstract void ApplyMaterial(Transform target, Material material, int depth);

        protected override Color GetRawColor(int id)
            => Original.GetColor(id);

        protected override void SetRawColor(int id, Color value)
            => Copy.SetColor(id, value);

        protected override float GetRawFloat(int id)
            => Original.GetFloat(id);

        protected override void SetRawFloat(int id, float value)
            => Copy.SetFloat(id, value);

        private void ApplyMaterial(Material material)
        {
            ApplyMaterial(transform, material, _depth);
        }

        private void DestroyCopy()
        {
            if (_copy != null)
            {
                _copy.Destroy();
                _copy = null;
            }
        }

        public void Dispose()
        {
            DestroyCopy();
        }

        #region Unity methods

        private void Start()
        {
            ApplyMaterial(Current);
        }

        private void OnDestroy()
        {
            DestroyCopy();

            ApplyMaterial(Current);
        }

#if UNITY_EDITOR
        private Material _cached;

        private void OnValidate()
        {
            if (_cached != _original)
            {
                DestroyCopy();

                _cached = _original;
            }

            ApplyMaterial(Current);
        }

        private void Reset()
        {
            _original = ReadMaterial(transform, _depth);
            _cached = _original;
        }
#endif

        #endregion
    }

    
}