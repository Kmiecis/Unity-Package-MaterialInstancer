using UnityEngine;

namespace Common.Materials
{
    [ExecuteAlways]
    public abstract class MaterialInstance : MonoBehaviour
    {
        [SerializeField] private Material _original;
        [SerializeField] private int _depth;

        private Material _copy;

        public Material Original
        {
            get => _original;
            set
            {
                DestroyCopy();

                _original = value;
                ApplyMaterial(_original);
            }
        }

        public Material Copy
        {
            get
            {
                if (_copy == null && _original != null)
                {
                    _copy = new Material(_original);
                    _copy.name += " (Clone)";
                    ApplyMaterial(_copy);
                }
                return _copy;
            }
        }

        public Material Current
        {
            get => _copy == null ? _original : _copy;
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

        private void DestroyCopy()
        {
            if (_copy != null)
            {
                _copy.Destroy();
                _copy = null;
            }
        }

        private void Start()
        {
            ApplyMaterial(Current);
        }

        private void OnDestroy()
        {
            DestroyCopy();

            ApplyMaterial(Current);
        }

#if UNITY_EDITOR
        private Material _cached;

        private void OnValidate()
        {
            if (_cached != _original)
            {
                DestroyCopy();

                _cached = _original;
            }

            ApplyMaterial(Current);
        }

        private void Reset()
        {
            _original = ReadMaterial(transform, _depth);
            _cached = _original;
        }
#endif
    }
}
