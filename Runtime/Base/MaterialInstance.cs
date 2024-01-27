using UnityEngine;

namespace Common.Materials
{
    [ExecuteAlways]
    public abstract class MaterialInstance : MonoBehaviour
    {
        [SerializeField] private Material _original;
        [SerializeField] private int _depth;

        private Material _clone;

        public Material Original
        {
            get => _original;
            set
            {
                DestroyClone();

                _original = value;

                ApplyMaterial(_original);
            }
        }

        public Material Clone
        {
            get
            {
                if (_clone == null && _original != null)
                {
                    _clone = CreateClone();

                    ApplyMaterial(_clone);
                }
                return _clone;
            }
        }

        public Material Current
        {
            get => HasClone ? _clone : _original;
        }

        public bool HasClone
        {
            get => _clone != null;
        }

        public void Apply()
        {
            ApplyMaterial(Current);
        }

        protected abstract Material ReadMaterial(Transform target, int depth);

        protected abstract void ApplyMaterial(Transform target, Material material, int depth);

        private void ApplyMaterial(Material material)
        {
            ApplyMaterial(transform, material, _depth);
        }

        private Material CreateClone()
        {
            var result = new Material(_original);
            result.name += " (Clone)";
            return result;
        }

        private void DestroyClone()
        {
            if (_clone != null)
            {
                _clone.Destroy();
                _clone = null;
            }
        }

        private void Start()
        {
            ApplyMaterial(Current);
        }

        private void OnDestroy()
        {
            DestroyClone();

            ApplyMaterial(Current);
        }

#if UNITY_EDITOR
        private Material _cached;

        private void OnValidate()
        {
            if (_cached != _original)
            {
                DestroyClone();

                _cached = _original;
            }

            ApplyMaterial(Current);
        }

        private void Reset()
        {
            _depth = transform.GetDepth();
            _original = ReadMaterial(transform, _depth);

            _cached = _original;
        }
#endif
    }
}
