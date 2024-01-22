using UnityEngine;

namespace Common.Materials
{
    [AddComponentMenu(nameof(Common) + "/" + nameof(Materials) + "/" + nameof(MaterialSwitcherForGraphic))]
    public class MaterialSwitcherForGraphic : MaterialSwitcher
    {
        protected override Material GetMaterial(Transform target, int depth)
        {
            return UMaterial.GetMaterialFromGraphic(target, depth);
        }

        protected override void SetMaterial(Transform target, Material material, int depth)
        {
            UMaterial.SetMaterialToGraphic(target, material, depth);
        }
    }
}