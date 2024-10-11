using UnityEngine;

namespace Common.Materials
{
    [ExecuteAlways]
    public abstract class MaterialProperty<T> : MaterialPropertyBase<T>
    {
        protected abstract void ApplyPropertyValue(Material material, T value);

        protected abstract T ReadPropertyValue(Material material);

        protected override void ApplyPropertyValue(T value)
        {
            foreach (var instance in GetInstances())
            {
                if (instance.GetClone(out var clone))
                {
                    ApplyPropertyValue(clone, value);
                }
            }
        }

        protected override void RestorePropertyValues()
        {
            foreach (var instance in GetInstances())
            {
                if (instance.TryGetClone(out var clone))
                {
                    var defaultValue = ReadPropertyValue(instance.Source);
                    ApplyPropertyValue(clone, defaultValue);
                }
            }
        }
    }
}
