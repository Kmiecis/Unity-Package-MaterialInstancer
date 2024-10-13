using UnityEngine;
using UnityEngine.Serialization;

namespace Common.Materials
{
    [ExecuteAlways]
    public abstract class MaterialInstance : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [FormerlySerializedAs("_original")]
        [SerializeField] private Material _source;
        [SerializeField] private int _depth;

        private Material _clone;

        public Transform Target
        {
            get => _target ?? transform;
            set => _target = value;
        }

        public Material Source
        {
            get => _source;
        }

        public Material Current
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
            if (CreateClone())
            {
                ApplyMaterial();
            }
        }

        public void ClearClone()
        {
            if (_clone != null)
            {
                RemoveMaterial();

                DestroyClone();
            }
        }

        public void Apply()
        {
            ApplyMaterial();
        }

        protected abstract void ApplyMaterial(Transform target, Material material, int depth);

        protected abstract void RemoveMaterial(Material material, int depth);

        private void ApplyMaterial()
        {
            ApplyMaterial(Target, Current, _depth);
        }

        private void RemoveMaterial()
        {
            RemoveMaterial(Current, _depth);
        }

        private bool CreateClone()
        {
            if (_clone == null && _source != null)
            {
                _clone = CreateClone(_source);

                return true;
            }
            return false;
        }

        private void DestroyClone()
        {
            _clone.Destroy();
            _clone = null;
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
            _target = transform;
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
