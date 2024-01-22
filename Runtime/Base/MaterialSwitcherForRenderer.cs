using UnityEngine;

namespace Common.Materials
{
    [AddComponentMenu(nameof(Common) + "/" + nameof(Materials) + "/" + nameof(MaterialSwitcherForRenderer))]
    public class MaterialSwitcherForRenderer : MaterialSwitcher
    {
        [SerializeField] private int _index;

        protected override Material GetMaterial(Transform target, int depth)
        {
            return UMaterial.GetMaterialFromRenderer(target, depth);
        }

        protected override void SetMaterial(Transform target, Material material, int depth)
        {
            UMaterial.SetMaterialToRenderer(target, material, _index, depth);
        }
    }
}