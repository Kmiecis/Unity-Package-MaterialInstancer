using UnityEngine;

namespace Common.Materials
{
    [AddComponentMenu(nameof(Common) + "/" + nameof(Materials) + "/" + "Material Integer")]
    public class MaterialInteger : MaterialPropertyNamed<int>
    {
        public override bool CanHandleProperty(Material material, string name)
#if UNITY_2021
            => material.HasInteger(name);
#else
            => material.HasInt(name);
#endif

        protected override void ApplyPropertyValue(Material material, int id, int value)
#if UNITY_2021
            => material.SetInteger(id, value);
#else
            => material.SetInt(id, value);
#endif

        protected override int ReadPropertyValue(Material material, int id)
#if UNITY_2021
            => material.GetInteger(id);
#else
            => material.GetInt(id);
#endif
    }
}
