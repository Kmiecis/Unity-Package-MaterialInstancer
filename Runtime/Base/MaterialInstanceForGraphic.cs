using UnityEngine;

namespace Common.Materials
{
    [AddComponentMenu(nameof(Common) + "/" + nameof(Materials) + "/" + nameof(MaterialInstanceForGraphic))]
    public class MaterialInstanceForGraphic : MaterialInstance
    {
        protected override Material ReadMaterial(Transform target, int depth)
        {
            return UMaterial.GetMaterialFromGraphic(target, depth);
        }

        protected override void ApplyMaterial(Transform target, Material material, int depth)
        {
            UMaterial.SetMaterialToGraphic(transform, material, depth);
        }
    }
}