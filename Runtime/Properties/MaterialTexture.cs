using UnityEngine;

namespace Common.Materializer
{
    [AddComponentMenu(nameof(Common) + "/" + nameof(Materializer) + "/" + nameof(MaterialTexture))]
    public class MaterialTexture : AMaterialValue<Texture>
    {
        protected override void ApplyPropertyValue(Material material, int id, Texture value)
        {
            material.SetTexture(id, value);
        }

        protected override Texture ReadPropertyValue(Material material, int id)
        {
            return material.GetTexture(id);
        }
    }
}
