using UnityEngine;

namespace Common.Materials
{
    [AddComponentMenu(nameof(Common) + "/" + nameof(Materials) + "/" + "Material Vector3")]
    public class MaterialVector3 : MaterialPropertyNamed<Vector3>
    {
        public override bool CanHandleProperty(Material material, string name)
            => material.HasVector(name);

        protected override void ApplyPropertyValue(Material material, int id, Vector3 value)
            => material.SetVector(id, value);

        protected override Vector3 ReadPropertyValue(Material material, int id)
            => material.GetVector(id);
    }
}
