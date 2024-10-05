using UnityEngine;
using UnityEngine.Serialization;

namespace Common.Materials
{
    [ExecuteAlways]
    public abstract class MaterialInstance : MonoBehaviour
    {
        [FormerlySerializedAs("_original")]
        [SerializeField] private Material _source;
        [SerializeField] private int _depth;

        private Material _clone;

        public Material Source
        {
            get => _source;
        }

        private Material Current
        {
            get => _clone != null ? _clone : _source;
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

        public void ClearClone()
        {
            RemoveMaterial();

            DestroyClone();
        }

        public void Apply()
        {
            ApplyMaterial();
        }

        protected abstract void ApplyMaterial(Transform target, Material material, int depth);

        protected abstract void RemoveMaterial(Material material, int depth);

        private void ApplyMaterial()
        {
            if (_clone != null)
            {
                ApplyMaterial(transform, _clone, _depth);
            }
        }

        private void RemoveMaterial()
        {
            if (_clone != null)
            {
                RemoveMaterial(_clone, _depth);
            }
        }

        private void CreateClone()
        {
            if (_clone == null && _source != null)
            {
                _clone = CreateClone(_source);
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

        private void OnDestroy()
        {
            ClearClone();
        }

#if UNITY_EDITOR
        private Material _cached;

        private void OnValidate()
        {
            if (_cached != _source)
            {
                _cached = _source;
            }

            if (_clone != null)
            {
                ClearClone();

                MakeClone();
            }
        }

        private void Reset()
        {
            _depth = transform.GetDepth();
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
