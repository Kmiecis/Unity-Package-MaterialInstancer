using UnityEngine;

namespace Common.Materializer
{
    public class MaterialChanger : MonoBehaviour
    {
        [SerializeField]
        protected MaterialInstance _instance;
        [SerializeField]
        protected Material _material;

        private Material _swapped;

        public bool IsChanged
        {
            get => _swapped is not null;
        }

        public void Apply()
        {
            _swapped = _instance.Original;
            _instance.Original = _material;
        }

        public void Revert()
        {
            _instance.Original = _swapped;
            _swapped = null;
        }

#if UNITY_EDITOR
        protected virtual void Reset()
        {
            if (_instance is null)
            {
                if (
                    transform.TryGetComponentInParent<MaterialInstance>(out var instance) ||
                    transform.TryGetComponentInChildren<MaterialInstance>(out instance)
                )
                {
                    _instance = instance;
                }
            }
        }
#endif
    }
}
