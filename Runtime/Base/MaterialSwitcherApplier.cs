using UnityEngine;

namespace Common.Materials
{
    [AddComponentMenu(nameof(Common) + "/" +nameof(Materials) + "/" + nameof(MaterialSwitcherApplier))]
    public class MaterialSwitcherApplier : MonoBehaviour
    {
        [SerializeField] private MaterialApplier _original;
        [SerializeField] private MaterialApplier _switched;
        [Space]
        [SerializeField] private bool _isSwitched;

        public MaterialApplier Current
        {
            get => _isSwitched ? _switched : _original;
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
            Current?.ApplyCurrent();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            ApplyCurrentInstance();
        }
#endif
    }
}
