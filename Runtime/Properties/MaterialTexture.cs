using UnityEngine;

namespace Common.Materials
{
    [AddComponentMenu(nameof(Common) + "/" + nameof(Materials) + "/Material Texture")]
    public class MaterialTexture : MaterialPropertyNamed<Texture>
    {
        public override bool CanHandleProperty(Material material, string name)
            => material.HasTexture(name);

        protected override void ApplyPropertyValue(Material material, int id, Texture value)
            => material.SetTexture(id, value);

        protected override Texture ReadPropertyValue(Material material, int id)
            => material.GetTexture(id);
    }
}
