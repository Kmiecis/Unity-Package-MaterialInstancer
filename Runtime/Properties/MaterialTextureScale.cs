using UnityEngine;

namespace Common.Materials
{
    [AddComponentMenu(nameof(Common) + "/" + nameof(Materials) + "/" + nameof(MaterialTextureScale))]
    public class MaterialTextureScale : MaterialPropertyNamed<Vector2>
    {
        public override bool CanHandleProperty(Material material, string name)
            => material.HasTexture(name);

        protected override void ApplyPropertyValue(Material material, int id, Vector2 value)
            => material.SetTextureScale(id, value);

        protected override Vector2 ReadPropertyValue(Material material, int id)
            => material.GetTextureScale(id);

        protected override Vector2 GetDefaultValue()
            => Vector2.one;
    }
}
