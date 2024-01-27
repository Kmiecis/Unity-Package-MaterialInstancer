using UnityEngine;

namespace Common.Materials
{
    [AddComponentMenu(nameof(Common) + "/" + nameof(Materials) + "/" + nameof(MaterialTextureOffset))]
    public class MaterialTextureOffset : MaterialPropertyNamed<Vector2>
    {
        protected override void ApplyPropertyValue(Material material, int id, Vector2 value)
            => material.SetTextureOffset(id, value);

        protected override Vector2 ReadPropertyValue(Material material, int id)
            => material.GetTextureOffset(id);
    }
}
