using UnityEngine;

namespace Common.Materials
{
    [AddComponentMenu(nameof(Common) + "/" +nameof(Materials) + "/" + nameof(MaterialSwitcher))]
    public class MaterialSwitcher : MonoBehaviour
    {
        [SerializeField]
        protected MaterialInstance _instance;
        [SerializeField]
        protected Material _material;

        private Material _swapped;

        public bool IsSwitched
        {
            get => _swapped != null;
            set => SetSwitched(value);
        }

        public void Switch()
        {
            _swapped = _instance.Original;
            _instance.Original = _material;
        }

        public void Revert()
        {
            _instance.Original = _swapped;
            _swapped = null;
        }

        public void SetSwitched(bool value)
        {
            if (value)
            {
                if (!IsSwitched)
                {
                    Switch();
                }
            }
            else
            {
                if (IsSwitched)
                {
                    Revert();
                }
            }
        }

#if UNITY_EDITOR
        protected virtual void Reset()
        {
            if (_instance == null)
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
