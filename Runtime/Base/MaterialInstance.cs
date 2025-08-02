using UnityEngine;

namespace Common.Materials
{
    [ExecuteAlways]
    [AddComponentMenu(nameof(Common) + "/" + nameof(Materials) + "/Material Instance")]
    public class MaterialInstance : MonoBehaviour
    {
        [SerializeField] private Material _source = null;

        private Material _clone;

        public Material Source
        {
            get => _source;
        }

        public Material Current
        {
            get => _clone != null ? _clone : _source;
        }

        public bool HasClone()
        {
            return _clone != null;
        }

        public bool CreateClone(out Material clone)
        {
            clone = default;
            return MakeClone() && TryGetClone(out clone);
        }

        public Material GetClone()
        {
            MakeClone();
            return _clone;
        }

        public bool GetClone(out Material clone)
        {
            MakeClone();
            return TryGetClone(out clone);
        }

        public bool TryGetClone(out Material clone)
        {
            return (clone = _clone) != null;
        }

        public bool MakeClone()
        {
            if (_clone == null && _source != null)
            {
                _clone = UMaterial.Create(_source);

                return true;
            }
            return false;
        }

        public bool ClearClone()
        {
            if (_clone != null)
            {
                _clone.Destroy();
                _clone = null;

                return true;
            }
            return false;
        }

        private void OnDestroy()
        {
            ClearClone();
        }
    }
}