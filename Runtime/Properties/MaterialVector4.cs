using UnityEngine;

namespace Common.Materials
{
    [AddComponentMenu(nameof(Common) + "/" + nameof(Materials) + "/Material Vector4")]
    public class MaterialVector4 : MaterialPropertyNamed<Vector4>
    {
        public override bool CanHandleProperty(Material material, string name)
            => material.HasVector(name);

        protected override void ApplyPropertyValue(Material material, int id, Vector4 value)
            => material.SetVector(id, value);

        protected override Vector4 ReadPropertyValue(Material material, int id)
            => material.GetVector(id);
    }
}
