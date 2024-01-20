using UnityEngine;

namespace Common.Materials
{
    public abstract class MaterialProperty<T> : MonoBehaviour
    {
        [SerializeField] protected MaterialInstance _instance = null;

        [SerializeField] protected string _name;
        [SerializeField] protected T _value;

        protected int _id;
        protected T _cached;

        public Material MaterialOriginal
        {
            get => _instance != null ? _instance.Original : null;
        }

        public Material MaterialCopy
        {
            get => _instance != null ? _instance.Copy : null;
        }

        public Material MaterialCurrent
        {
            get => _instance != null ? _instance.Current : null;
        }

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                RefreshPropertyId();
            }
        }

        public T Value
        {
            get => _value;
            set => ApplyPropertyValue(value);
        }

        public int Id
        {
            get => _id;
        }

        protected abstract void ApplyPropertyValue(Material material, int id, T value);

        protected abstract T ReadPropertyValue(Material material, int id);

        private void ApplyPropertyValue(T value)
        {
            ApplyPropertyValue(MaterialCopy, _id, value);

            _value = value;
            _cached = value;
        }

        private T ReadPropertyValue(Material target)
        {
            return ReadPropertyValue(target, _id);
        }

        private bool TryReadPropertyValue(out T value)
        {
            var target = MaterialCurrent;
            if (target != null &&
                target.HasProperty(_id))
            {
                value = ReadPropertyValue(target, _id);
                return true;
            }

            value = default;
            return false;
        }

        private void ApplyPropertyValueIfDirty(T value)
        {
            if (!Equals(_cached, default(T)) &&
                !Equals(_cached, value))
            {
                ApplyPropertyValue(value);
            }
        }

        private void TryCachePropertyValue()
        {
            if (TryReadPropertyValue(out var value))
            {
                _cached = value;
            }
        }

        private void RefreshPropertyId()
        {
            _id = Shader.PropertyToID(_name);
        }

        private void Start()
        {
            RefreshPropertyId();
            TryCachePropertyValue();

            ApplyPropertyValueIfDirty(_value);
        }

        private void OnDidApplyAnimationProperties()
        {
            ApplyPropertyValueIfDirty(_value);
        }

#if UNITY_EDITOR
        protected void OnValidate()
        {
            RefreshPropertyId();
            TryCachePropertyValue();

            ApplyPropertyValueIfDirty(_value);
        }

        protected virtual void Reset()
        {
            if (_instance == null)
            {
                if (transform.TryGetComponentInParent<MaterialInstance>(out var instance) ||
                    transform.TryGetComponentInChildren<MaterialInstance>(out instance))
                {
                    _instance = instance;
                }
            }
        }
#endif
    }
}
