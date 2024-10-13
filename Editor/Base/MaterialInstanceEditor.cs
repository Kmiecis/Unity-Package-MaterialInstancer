using UnityEditor;
using UnityEngine;

namespace Common.Materials
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(MaterialInstance), true)]
    public class MaterialInstanceEditor : Editor
    {
        private MaterialInstance Script
            => (MaterialInstance)target;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (Script.TryGetClone(out _))
            {
                if (GUILayout.Button("Clear"))
                {
                    Script.ClearClone();
                }
            }
            else
            {
                if (GUILayout.Button("Apply"))
                {
                    Script.MakeClone();
                }
            }
        }
    }
}