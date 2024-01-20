using UnityEngine;

namespace Common.Materials
{
    [AddComponentMenu(nameof(Common) + "/" +nameof(Materials) + "/" + nameof(MaterialInstanceSwitcher))]
    public class MaterialInstanceSwitcher : MonoBehaviour
    {
        [SerializeField] private MaterialInstance _instance;
        [SerializeField] private Material _material;

        private Material _switched;

        public bool IsSwitched
        {
            get => _switched != null;
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
            _switched = _instance.Original;
            _instance.Original = _material;
        }

        public void SetOriginal()
        {
            _instance.Original = _switched;
            _switched = null;
        }

#if UNITY_EDITOR
        private void Reset()
        {
            if (transform.TryGetComponentInParent<MaterialInstance>(out var instance) ||
                transform.TryGetComponentInChildren<MaterialInstance>(out instance))
            {
                _instance = instance;
            }
        }
#endif
    }
}
