using UnityEngine;

namespace Common.Materials
{
    [AddComponentMenu(nameof(Common) + "/" + nameof(Materials) + "/Material Instance")]
    public class MaterialInstance : MaterialReference
    {
        private Material _clone;

        public override Material Material
        {
            get => GetClone();
        }

        private Material GetClone()
        {
            if (_clone == null && _source != null)
            {
                _clone = UMaterial.Create(_source);
            }
            return _clone;
        }

        private void ClearClone()
        {
            if (_clone != null)
            {
                _clone.Destroy();
                _clone = null;
            }
        }

        private void OnDestroy()
        {
            ClearClone();
        }
    }
}