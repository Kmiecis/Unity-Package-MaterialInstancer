using System;
using UnityEngine;

namespace Common.Materials
{
    [Obsolete("Replace with " + nameof(MaterialReferenceSwitcher))]
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
        }

        public void SetOriginal()
        {
            _isSwitched = false;
        }
    }
}
