using UnityEngine;

namespace Common.Materializer
{
    public class MaterialColor : AMaterialValue<Color>
    {
        protected override void ApplyPropertyValue(Material material, int id, Color value)
        {
            material.SetColor(id, value);
        }

        protected override Color ReadPropertyValue(Material material, int id)
        {
            return material.GetColor(id);
        }

#if UNITY_EDITOR
        protected override void Reset()
        {
            base.Reset();

            PropertyValue = Color.white;
        }
#endif
    }
}
