using UnityEngine;

namespace Common.Materials
{
    [AddComponentMenu(nameof(Common) + "/" + nameof(Materials) + "/" + nameof(MaterialVector3))]
    public class MaterialVector3 : MaterialPropertyNamed<Vector3>
    {
        protected override void ApplyPropertyValue(Material material, int id, Vector3 value)
        {
            material.SetVector(id, value);
        }

        protected override Vector3 ReadPropertyValue(Material material, int id)
        {
            return material.GetVector(id);
        }
    }
}
