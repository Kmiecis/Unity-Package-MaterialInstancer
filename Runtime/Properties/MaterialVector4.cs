using UnityEngine;

namespace Common.Materials
{
    [AddComponentMenu(nameof(Common) + "/" + nameof(Materials) + "/" + nameof(MaterialVector4))]
    public class MaterialVector4 : MaterialPropertyNamed<Vector4>
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
