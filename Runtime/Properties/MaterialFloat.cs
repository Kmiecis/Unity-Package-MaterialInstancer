using UnityEngine;

namespace Common.Materials
{
    [AddComponentMenu(nameof(Common) + "/" + nameof(Materials) + "/Material Float")]
    public class MaterialFloat : MaterialPropertyNamed<float>
    {
        public override bool CanHandleProperty(Material material, string name)
            => material.HasFloat(name);

        protected override void ApplyPropertyValue(Material material, int id, float value)
            => material.SetFloat(id, value);

        protected override float ReadPropertyValue(Material material, int id)
            => material.GetFloat(id);
    }
}
