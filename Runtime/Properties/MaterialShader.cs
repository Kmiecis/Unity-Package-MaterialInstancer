using UnityEngine;

namespace Common.Materials
{
    [AddComponentMenu(nameof(Common) + "/" + nameof(Materials) + "/" + nameof(MaterialShader))]
    public class MaterialShader : MaterialProperty<Shader>
    {
        protected override void ApplyPropertyValue(Material material, Shader value)
        {
            material.shader = value;
        }

        protected override Shader ReadPropertyValue(Material material)
        {
            return material.shader;
        }
    }
}
