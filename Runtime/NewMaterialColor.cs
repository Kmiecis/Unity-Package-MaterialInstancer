using UnityEngine;

namespace Common.Materials
{
    [AddComponentMenu(nameof(Common) + "/" + nameof(Materials) + "/" + nameof(NewMaterialColor))]
    public class NewMaterialColor : NewMaterialProperty<Color>
    {
        protected override void ApplyPropertyValue(IPropertiesInstance instance, int id, Color value)
            => instance.SetColor(id, value);

        protected override Color ReadPropertyValue(IPropertiesInstance instance, int id)
            => instance.GetColor(id);

        protected override Color GetDefaultValue()
            => Color.white;
    }
}