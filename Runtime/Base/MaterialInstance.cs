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
            set => SetOriginal(value);
        }

        public Material Current
        {
            get => _clone != null ? _clone : _original;
        }

        public Material GetClone()
        {
            GetClone(out var result);
            return result;
        }

        public bool GetClone(out Material clone)
        {
            MakeClone();
            return TryGetClone(out clone);
        }

        public bool TryGetClone(out Material clone)
        {
            clone = _clone;
            return clone != null;
        }

        public void MakeClone()
        {
            CreateClone();

            ApplyMaterial();
        }

        public void Apply()
        {
            ApplyMaterial();
        }

        public void ClearClone()
        {
            DestroyClone();

            ApplyMaterial();
        }

        protected abstract Material ReadMaterial(Transform target, int depth);

        protected abstract void ApplyMaterial(Transform target, Material material, int depth);

        private void ApplyMaterial()
        {
            ApplyMaterial(transform, Current, _depth);
        }

        private void SetOriginal(Material value)
        {
            DestroyClone();

            _original = value;

            ApplyMaterial();
        }

        private void CreateClone()
        {
            if (_clone == null && _original != null)
            {
                _clone = CreateClone(_original);
            }
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
            ApplyMaterial();
        }

        private void OnDestroy()
        {
            DestroyClone();

            ApplyMaterial();
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

            ApplyMaterial();
        }

        private void Reset()
        {
            _depth = transform.GetDepth();
            _original = ReadMaterial(transform, _depth);

            _cached = _original;
        }
#endif

        private static Material CreateClone(Material source)
        {
            var result = new Material(source);
            result.name += " (Clone)";
            result.hideFlags = HideFlags.DontSave;
            return result;
        }
    }
}
