using UnityEngine;

namespace Common.Materials
{
    [AddComponentMenu(nameof(Common) + "/" + nameof(Materials) + "/" + nameof(MaterialFloat))]
    public class MaterialFloat : MaterialPropertyNamed<float>
    {
        protected override void ApplyPropertyValue(Material material, int id, float value)
            => material.SetFloat(id, value);

        protected override float ReadPropertyValue(Material material, int id)
            => material.GetFloat(id);
    }
}
