using System.Collections.Generic;
using UnityEngine;

namespace Common.Materials
{
    public abstract class MaterialApplier : MonoBehaviour
    {
        [SerializeField] private MaterialInstance[] _instances;
        [SerializeField] private Transform _target;
        [SerializeField] private int _depth;

        public Transform Target
        {
            get => _target ?? transform;
            set => SetTarget(value);
        }

        public bool HasClone()
        {
            foreach (var instance in GetInstances())
            {
                if (instance.TryGetClone(out _))
                {
                    return true;
                }
            }
            return false;
        }

        public void ApplyClone()
        {
            foreach (var instance in GetInstances())
            {
                if (instance.GetClone(out var material))
                {
                    ApplyMaterial(Target, material, _depth);
                }
            }
        }

        public void ReapplyClone()
        {
            foreach (var instance in GetInstances())
            {
                if (instance.TryGetClone(out var material))
                {
                    ApplyMaterial(Target, material, _depth);
                }
            }
        }

        public void ApplyCurrent()
        {
            foreach (var instance in GetInstances())
            {
                var material = instance.Current;

                ApplyMaterial(Target, material, _depth);
            }
        }

        public void RemoveClone()
        {
            foreach (var instance in GetInstances())
            {
                if (instance.TryGetClone(out var material))
                {
                    RemoveMaterial(Target, material, _depth);
                }
            }
        }

        public void ClearClone()
        {
            foreach (var instance in GetInstances())
            {
                if (instance.TryGetClone(out var material))
                {
                    RemoveMaterial(Target, material, _depth);
                    
                    instance.ClearClone();
                }
            }
        }

        public IEnumerable<MaterialInstance> GetInstances()
        {
            if (_instances != null)
            {
                for (int i = 0; i < _instances.Length; ++i)
                {
                    yield return _instances[i];
                }
            }
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

        private void OnDestroy()
        {
            ClearClone();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            ClearClone();
        }

        private void Reset()
        {
            _instances = GetComponentsInChildren<MaterialInstance>();
            _target = transform;
            _depth = transform.GetDepth();

            ReapplyClone();
        }
#endif
    }
}