using UnityEngine;

namespace Common.Materials
{
    [AddComponentMenu(nameof(Common) + "/" + nameof(Materials) + "/" + nameof(MaterialAlpha))]
    public class MaterialAlpha : MaterialPropertyNamed<float>
    {
        public override bool CanHandleProperty(Material material, string name)
        {
            return material.HasColor(name);
        }

        protected override void ApplyPropertyValue(Material material, int id, float value)
        {
            var color = material.GetColor(id);
            color.a = value;
            material.SetColor(id, color);
        }

        protected override float ReadPropertyValue(Material material, int id)
        {
            var color = material.GetColor(id);
            return color.a;
        }

        protected override float GetDefaultValue()
        {
            return 1.0f;
        }
    }
}