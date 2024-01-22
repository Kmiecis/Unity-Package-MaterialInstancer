using UnityEngine;

namespace Common.Materials
{
    [AddComponentMenu(nameof(Common) + "/" +nameof(Materials) + "/" + nameof(MaterialInstanceSwitcher))]
    public class MaterialInstanceSwitcher : MonoBehaviour
    {
        [SerializeField] private MaterialInstance _original;
        [SerializeField] private MaterialInstance _switched;
        [SerializeField] private bool _isSwitched;

        public MaterialInstance Current
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
            Current?.Apply();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            ApplyCurrentInstance();
        }

        private void Reset()
        {
            _original = transform.GetComponentInChildren<MaterialInstance>();
        }
#endif
    }
}
