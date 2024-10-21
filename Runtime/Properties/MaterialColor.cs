using UnityEngine;

namespace Common.Materials
{
    [AddComponentMenu(nameof(Common) + "/" + nameof(Materials) + "/" + "Material Color")]
    public class MaterialColor : MaterialPropertyNamed<Color>
    {
        public float Alpha
        {
            get => GetAlpha();
            set => SetAlpha(value);
        }

        public float GetAlpha()
        {
            var color = GetValue();
            return color.a;
        }

        public void SetAlpha(float a)
        {
            var color = new Color(_value.r, _value.g, _value.b, a);
            SetValue(color);
        }

        public override bool CanHandleProperty(Material material, string name)
            => material.HasColor(name);

        protected override void ApplyPropertyValue(Material material, int id, Color value)
            => material.SetColor(id, value);

        protected override Color ReadPropertyValue(Material material, int id)
            => material.GetColor(id);

        protected override Color GetDefaultValue()
            => Color.white;
    }
}
