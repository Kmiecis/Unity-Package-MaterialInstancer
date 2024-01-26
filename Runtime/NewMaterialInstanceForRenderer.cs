using UnityEngine;

namespace Common.Materials
{
    [AddComponentMenu(nameof(Common) + "/" + nameof(Materials) + "/" + nameof(NewMaterialInstanceForRenderer))]
    public class NewMaterialInstanceForRenderer : NewMaterialInstance
    {
        [SerializeField] private int _index;

        protected override Material ReadMaterial(Transform target, int depth)
            => UMaterial.GetMaterialFromRenderer(target, depth);

        protected override void ApplyMaterial(Transform target, Material material, int depth)
            => UMaterial.SetMaterialToRenderer(target, material, _index, depth);
    }
}