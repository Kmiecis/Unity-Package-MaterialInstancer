using UnityEngine;

namespace Common.Materials
{
    [AddComponentMenu(nameof(Common) + "/" + nameof(Materials) + "/" + nameof(MaterialColor))]
    public class MaterialColor : MaterialPropertyNamed<Color>
    {
        protected override void ApplyPropertyValue(Material material, int id, Color value)
            => material.SetColor(id, value);

        protected override Color ReadPropertyValue(Material material, int id)
            => material.GetColor(id);

        protected override Color GetDefaultValue()
            => Color.white;
    }
}
