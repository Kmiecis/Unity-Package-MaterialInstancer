using UnityEngine;

namespace Common.Materials
{
    [ExecuteAlways]
    [AddComponentMenu(nameof(Common) + "/" + nameof(Materials) + "/" + nameof(MaterialInstance))]
    public class MaterialInstance : MonoBehaviour
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
                ApplyMaterial(Current);
            }
        }

        public Material Copy
        {
            get
            {
                if (_copy == null && _original != null)
                {
                    _copy = new Material(_original);
                    ApplyMaterial(Current);
                }
                return _copy;
            }
        }

        public Material Current
        {
            get => _copy == null ? _original : _copy;
        }

        private void ApplyMaterial(Material material)
        {
            UMaterial.SetMaterial(material, transform, _depth);
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
            _original = UMaterial.GetMaterial(transform, _depth);
            _cached = _original;
        }
#endif
    }
}
