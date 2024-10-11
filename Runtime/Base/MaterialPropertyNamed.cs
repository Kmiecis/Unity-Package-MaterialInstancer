using UnityEngine;

namespace Common.Materials
{
    [ExecuteAlways]
    public abstract class MaterialPropertyNamed<T> : MaterialPropertyBase<T>
    {
        [SerializeField] protected string _name;

        private int _id;

        public string Name
        {
            get => _name;
            set => SetPropertyName(value);
        }

        protected abstract void ApplyPropertyValue(Material material, int id, T value);

        protected abstract T ReadPropertyValue(Material material, int id);

        private void SetPropertyName(string value)
        {
            _name = value;

            RefreshPropertyId();
            RefreshPropertyValue();
        }

        protected override void ApplyPropertyValue(T value)
        {
            foreach (var instance in GetInstances())
            {
                if (instance.GetClone(out var clone))
                {
                    ApplyPropertyValue(clone, _id, value);
                }
            }
        }

        protected override void RestorePropertyValues()
        {
            foreach (var instance in GetInstances())
            {
                if (instance.TryGetClone(out var clone))
                {
                    var defaultValue = ReadPropertyValue(instance.Source, _id);
                    ApplyPropertyValue(clone, _id, defaultValue);
                }
            }
        }

        private void RefreshPropertyId()
        {
            _id = Shader.PropertyToID(_name);
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            RefreshPropertyId();

            base.OnValidate();
        }
#endif
    }
}
