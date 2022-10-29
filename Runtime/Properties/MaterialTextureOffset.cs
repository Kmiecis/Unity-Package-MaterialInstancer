using UnityEngine;

namespace Common.Materializer
{
    [AddComponentMenu(nameof(Common) + "/" + nameof(Materializer) + "/" + nameof(MaterialTextureOffset))]
    public class MaterialTextureOffset : AMaterialValue<Vector2>
    {
        protected override void ApplyPropertyValue(Material material, int id, Vector2 value)
        {
            material.SetTextureOffset(id, value);
        }

        protected override Vector2 ReadPropertyValue(Material material, int id)
        {
            return material.GetTextureOffset(id);
        }
    }
}