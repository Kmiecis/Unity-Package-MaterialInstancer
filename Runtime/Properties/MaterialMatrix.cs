using UnityEngine;

namespace Common.Materials
{
    [AddComponentMenu(nameof(Common) + "/" + nameof(Materials) + "/" + nameof(MaterialMatrix))]
    public class MaterialMatrix : MaterialProperty<Matrix4x4>
    {
        protected override void ApplyPropertyValue(Material material, int id, Matrix4x4 value)
        {
            material.SetMatrix(id, value);
        }

        protected override Matrix4x4 ReadPropertyValue(Material material, int id)
        {
            return material.GetMatrix(id);
        }
    }
}
