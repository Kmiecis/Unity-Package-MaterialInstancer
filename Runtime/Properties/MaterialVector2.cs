using UnityEngine;

namespace Common.Materials
{
    [AddComponentMenu(nameof(Common) + "/" + nameof(Materials) + "/Material Vector2")]
    public class MaterialVector2 : MaterialPropertyNamed<Vector2>
    {
        public override bool CanHandleProperty(Material material, string name)
            => material.HasVector(name);

        protected override void ApplyPropertyValue(Material material, int id, Vector2 value)
            => material.SetVector(id, value);

        protected override Vector2 ReadPropertyValue(Material material, int id)
            => material.GetVector(id);
    }
}
