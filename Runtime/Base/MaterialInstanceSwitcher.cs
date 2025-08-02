using UnityEngine;

namespace Common.Materials
{
    [AddComponentMenu(nameof(Common) + "/" + nameof(Materials) + "/Material Instance Switcher")]
    public class MaterialInstanceSwitcher : MonoBehaviour
    {
        [SerializeField] private MaterialInstance _original;
        [SerializeField] private MaterialInstance _switched;
        [Space]
        [SerializeField] private bool _isSwitched;

        public MaterialInstance Current
        {
            get => _isSwitched ? _switched : _original;
        }

        public MaterialInstance Other
        {
            get => _isSwitched ? _original : _switched;
        }

        public bool IsSwitched
        {
            get => _isSwitched;
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
            _isSwitched = true;

            ApplyCurrentInstance();
        }

        public void SetOriginal()
        {
            _isSwitched = false;

            ApplyCurrentInstance();
        }

        private void ApplyCurrentInstance()
        {
            Other?.ClearClone();

            Current?.MakeClone();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            Current?.ClearClone();

            Current?.MakeClone();
        }
#endif
    }
}
