using UnityEngine;

namespace Common.Materials
{
    [AddComponentMenu(nameof(Common) + "/" + nameof(Materials) + "/Material Reference")]
    public class MaterialReference : MonoBehaviour
    {
        [SerializeField] protected Material _source;

        public Material Source
        {
            get => _source;
        }

        public virtual Material Material
        {
            get => _source;
        }
    }
}