using UnityEngine;

namespace Common.Materials
{
    [AddComponentMenu(nameof(Common) + "/" + nameof(Materials) + "/" + nameof(MaterialVector2))]
    public class MaterialVector2 : MaterialPropertyNamed<Vector2>
    {
        protected override void ApplyPropertyValue(Material material, int id, Vector2 value)
            => material.SetVector(id, value);

        protected override Vector2 ReadPropertyValue(Material material, int id)
            => material.GetVector(id);
    }
}
