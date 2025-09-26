using UnityEngine;

namespace Common.Materials
{
    public abstract class MaterialApplierToTarget : MaterialApplier
    {
        [SerializeField] private Transform _target;
        [SerializeField] private int _depth = -1;

        public Transform Target
        {
            get => _target != null ? _target : transform;
            set => SetTarget(value);
        }

        public int Depth
        {
            get => _depth;
            set => SetDepth(value);
        }

        public void SetTarget(Transform target)
        {
            Clear();

            _target = target;

            Apply();
        }

        public void SetDepth(int depth)
        {
            Clear();

            _depth = depth;

            Apply();
        }

        protected override void ApplyMaterial(Material material)
        {
            ApplyMaterial(Target, material, _depth);
        }

        protected override void RemoveMaterial(Material material)
        {
            RemoveMaterial(Target, material, _depth);
        }

        protected abstract void ApplyMaterial(Transform target, Material material);

        protected abstract void RemoveMaterial(Transform target, Material material);

        private void ApplyMaterial(Transform target, Material material, int depth)
        {
            ApplyMaterial(target, material);

            if (depth != 0)
            {
                foreach (Transform child in target)
                {
                    if (!child.TryGetComponent<MaterialBlocker>(out _))
                    {
                        ApplyMaterial(child, material, depth - 1);
                    }
                }
            }
        }

        private void RemoveMaterial(Transform target, Material material, int depth)
        {
            RemoveMaterial(target, material);

            if (depth != 0)
            {
                foreach (Transform child in target)
                {
                    if (!child.TryGetComponent<MaterialBlocker>(out _))
                    {
                        RemoveMaterial(child, material, depth - 1);
                    }
                }
            }
        }

#if UNITY_EDITOR
        protected override void Reset()
        {
            base.Reset();

            _target = transform;
        }
#endif
    }
}