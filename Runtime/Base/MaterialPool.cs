using System.Collections.Generic;
using UnityEngine;

namespace Common.Materials
{
    public class MaterialPool : ScriptableObject
    {
        [SerializeField] private Material _source;
        [SerializeField] private int _capacity = 1;

        private readonly Queue<Material> _materials;

        public MaterialPool()
        {
            _materials = new Queue<Material>();
        }

        public Material Borrow()
        {
            if (_materials.Count == 0)
            {
                return UMaterial.Create(_source);
            }
            return _materials.Dequeue();
        }

        public void Return(Material material)
        {
            if (_materials.Count >= _capacity)
            {
                material.Destroy();
            }
            else
            {
                _materials.Enqueue(material);
            }
        }

        public void Clear()
        {
            while (_materials.TryDequeue(out var material))
            {
                material.Destroy();
            }
        }
    }
}