using UnityEngine;

namespace Common.Materials
{
    [ExecuteAlways]
    public abstract class MaterialSwitcher : MonoBehaviour
    {
        [SerializeField] private Material _original;
        [SerializeField] private Material _switched;
        [SerializeField] private int _depth;
        [SerializeField] private bool _isSwithed;

        public Material Current
        {
            get => _isSwithed ? _switched : _original;
        }

        public bool IsSwitched
        {
            get => _isSwithed;
            set => SetSwitched(value);
        }

        public void SetSwitched(bool value)
        {
            if (value)
            {
                if (!IsSwitched)
                {
                    SetSwitched();
                }
            }
            else
            {
                if (IsSwitched)
                {
                    SetOriginal();
                }
            }
        }

        public void SetSwitched()
        {
            _isSwithed = true;
            SetCurrentMaterial();
        }

        public void SetOriginal()
        {
            _isSwithed = false;
            SetCurrentMaterial();
        }

        protected abstract Material GetMaterial(Transform target, int depth);

        protected abstract void SetMaterial(Transform target, Material material, int depth);

        private void SetCurrentMaterial()
        {
            SetMaterial(transform, Current, _depth);
        }

        private void Start()
        {
            SetCurrentMaterial();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            SetCurrentMaterial();
        }

        private void Reset()
        {
            _original = GetMaterial(transform, _depth);
        }
#endif
    }
}