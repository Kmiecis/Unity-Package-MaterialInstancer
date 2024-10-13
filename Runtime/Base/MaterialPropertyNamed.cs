using UnityEngine;

namespace Common.Materials
{
    public interface IMaterialPropertyNamedVerifier
    {
        bool CanHandleProperty(Material material, string name);
    }

    [ExecuteAlways]
    public abstract class MaterialPropertyNamed<T> : MaterialProperty<T>, IMaterialPropertyNamedVerifier
    {
        [SerializeField] protected string _name;

        private int _id;

        public string Name
        {
            get => _name;
            set => SetPropertyName(value);
        }

        public abstract bool CanHandleProperty(Material material, string name);

        protected abstract void ApplyPropertyValue(Material material, int id, T value);

        protected abstract T ReadPropertyValue(Material material, int id);

        protected override void ApplyPropertyValue(Material material, T value)
        {
            ApplyPropertyValue(material, _id, value);
        }

        protected override T ReadPropertyValue(Material material)
        {
            return ReadPropertyValue(material, _id);
        }

        private void SetPropertyName(string value)
        {
            _name = value;

            RefreshPropertyId();
            RefreshPropertyValue();
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
