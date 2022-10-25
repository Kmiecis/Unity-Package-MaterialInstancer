using UnityEngine;

namespace Common.Materializer
{
    public class MaterialVector : AMaterialValue<Vector4>
    {
        protected override void ApplyPropertyValue(Material material, int id, Vector4 value)
        {
            material.SetVector(id, value);
        }

        protected override Vector4 ReadPropertyValue(Material material, int id)
        {
            return material.GetVector(id);
        }
    }
}
