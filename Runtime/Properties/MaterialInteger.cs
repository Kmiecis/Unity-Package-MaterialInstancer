using UnityEngine;

namespace Common.Materials
{
    [AddComponentMenu(nameof(Common) + "/" + nameof(Materials) + "/" + nameof(MaterialInteger))]
    public class MaterialInteger : AMaterialValue<int>
    {
        protected override void ApplyPropertyValue(Material material, int id, int value)
        {
            material.SetInteger(id, value);
        }

        protected override int ReadPropertyValue(Material material, int id)
        {
            return material.GetInteger(id);
        }
    }
}
