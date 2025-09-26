using UnityEngine;

namespace Common.Materials
{
    [AddComponentMenu(nameof(Common) + "/" + nameof(Materials) + "/Material Reference Switcher")]
    public class MaterialReferenceSwitcher : MonoBehaviour
    {
        [SerializeField] private MaterialReference _original;
        [SerializeField] private MaterialReference _switched;
        [Space]
        [SerializeField] private bool _isSwitched;

        public Material Current
        {
            get => GetReference(_isSwitched).Material;
        }

        public Material Other
        {
            get => GetReference(!_isSwitched).Material;
        }

        public bool IsSwitched
        {
            get => _isSwitched;
            set => SetSwitched(value);
        }

        public Material SetSwitched(bool value)
        {
            if (value)
            {
                if (!IsSwitched)
                {
                    return SetSwitched();
                }
            }
            else
            {
                if (IsSwitched)
                {
                    return SetOriginal();
                }
            }
            return Current;
        }

        public Material SetSwitched()
        {
            _isSwitched = true;
            return _switched.Material;
        }

        public Material SetOriginal()
        {
            _isSwitched = false;
            return _original.Material;
        }

        private MaterialReference GetReference(bool switched)
        {
            return switched ? _switched : _original;
        }
    }
}
