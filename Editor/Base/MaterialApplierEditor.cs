using UnityEditor;
using UnityEngine;

namespace Common.Materials
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(MaterialApplier), true)]
    public class MaterialApplierEditor : Editor
    {
        private MaterialApplier Script
            => (MaterialApplier)target;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (Script.HasClone())
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
                    Script.ApplyClone();
                }
            }
        }
    }
}