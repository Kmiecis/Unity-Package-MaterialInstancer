using System.Collections.Generic;
using UnityEngine;

namespace Common.Materials
{
    public abstract class MaterialPropertyNamed<T> : MonoBehaviour
    {
        [SerializeField] protected MaterialInstance[] _instances = null;

        [SerializeField] protected string _name;
        [SerializeField] protected T _value;

        protected int _id;

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

        private IEnumerable<Material> GetMaterialCopies()
        {
            foreach (var instance in _instances)
            {
                yield return instance.Copy;
            }
        }

        private void ApplyPropertyValue()
        {
            ApplyPropertyValue(_value);
        }

        private void ApplyPropertyValue(T value)
        {
            foreach (var material in GetMaterialCopies())
            {
                ApplyPropertyValue(material, _id, value);
            }

            _value = value;
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

#if UNITY_EDITOR
        protected void OnValidate()
        {
            RefreshPropertyId();
            ApplyPropertyValue();
        }

        protected virtual void Reset()
        {
            _instances = transform.GetComponentsInChildren<MaterialInstance>();
        }
#endif
    }
}
