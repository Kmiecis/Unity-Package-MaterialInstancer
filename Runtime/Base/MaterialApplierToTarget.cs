using UnityEngine;

namespace Common.Materials
{
    public abstract class MaterialApplierToTarget : MaterialApplier
    {
        [SerializeField] private Transform _target;
        [SerializeField] private int _depth = -1;

        public Transform Target
        {
            get => _target ?? transform;
            set => SetTarget(value);
        }

        public int Depth
        {
            get => _depth;
            set => SetDepth(value);
        }

        protected override void ApplyMaterial(Material material)
        {
            ApplyMaterial(_target, material, _depth);
        }

        protected override void RemoveMaterial(Material material)
        {
            RemoveMaterial(_target, material, _depth);
        }

        protected abstract void ApplyMaterial(Transform target, Material material, int depth);

        protected abstract void RemoveMaterial(Transform target, Material material, int depth);

        private void SetTarget(Transform target)
        {
            if (HasClone())
            {
                ClearClone();

                _target = target;

                ApplyClone();
            }
        }

        private void SetDepth(int depth)
        {
            if (HasClone())
            {
                ClearClone();

                _depth = depth;

                ApplyClone();
            }
        }

#if UNITY_EDITOR
        protected override void Reset()
        {
            _target = transform;

            base.Reset();
        }
#endif
    }
}