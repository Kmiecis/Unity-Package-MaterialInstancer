using UnityEngine;

namespace Common.Materials
{
    [AddComponentMenu(nameof(Common) + "/" + nameof(Materials) + "/" + "Material Render Queue")]
    public class MaterialRenderQueue : MaterialProperty<int>
    {
        protected override void ApplyPropertyValue(Material material, int value)
            => material.renderQueue = value;

        protected override int ReadPropertyValue(Material material)
            => material.renderQueue;
    }
}
