using UnityEngine;

namespace Common.Materials
{
    [AddComponentMenu(nameof(Common) + "/" + nameof(Materials) + "/" + nameof(MaterialInstanceForRenderer))]
    public class MaterialInstanceForRenderer : MaterialInstance
    {
        [SerializeField] private int _index;

        protected override Material ReadMaterial(Transform target, int depth)
            => UMaterial.GetMaterialFromRenderer(target, depth);

        protected override void ApplyMaterial(Transform target, Material material, int depth)
            => UMaterial.SetMaterialToRenderer(target, material, _index, depth);
    }
}