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
            set => _target = value;
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

        public void ClearClone()
        {
            foreach (var instance in GetInstances())
            {
                if (instance.TryGetClone(out var material))
                {
                    RemoveMaterial(material, _depth);
                    
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

        protected abstract void RemoveMaterial(Material material, int depth);
        
#if UNITY_EDITOR
        private void OnDestroy()
        {
            ClearClone();
        }

        private void OnValidate()
        {
            ReapplyClone();
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