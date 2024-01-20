using UnityEngine;

namespace Common.Materials
{
    [ExecuteAlways]
    [AddComponentMenu(nameof(Common) + "/" + nameof(Materials) + "/" + nameof(MaterialSwitcher))]
    public class MaterialSwitcher : MonoBehaviour
    {
        [SerializeField] private Material _original;
        [SerializeField] private Material _switched;
        [SerializeField] private int _depth;
        [SerializeField] private bool _isSwithed = false;

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

        private void SetCurrentMaterial()
        {
            UMaterial.SetMaterial(Current, transform, _depth);
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
            _original = UMaterial.GetMaterial(transform, _depth);
        }
#endif
    }
}