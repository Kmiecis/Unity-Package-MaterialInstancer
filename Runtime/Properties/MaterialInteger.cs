using UnityEngine;

namespace Common.Materials
{
    [AddComponentMenu(nameof(Common) + "/" + nameof(Materials) + "/" + nameof(MaterialInteger))]
    public class MaterialInteger : AMaterialValue<int>
    {
        protected override void ApplyPropertyValue(Material material, int id, int value)
        {
#if UNITY_2021
            material.SetInteger(id, value);
#else
            material.SetInt(id, value);
#endif
        }

        protected override int ReadPropertyValue(Material material, int id)
        {
#if UNITY_2021
            return material.GetInteger(id);
#else
            return material.GetInt(id);
#endif
        }
    }
}
