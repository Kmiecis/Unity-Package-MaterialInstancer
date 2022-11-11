using Common.Materials;
using UnityEditor;
using UnityEngine;

namespace CommonEditor.Materials
{
    [CustomEditor(typeof(MaterialChanger))]
    public class MaterialChangerEditor : Editor
    {
        private MaterialChanger Script
        {
            get => (MaterialChanger)target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (Script.IsChanged)
            {
                if (GUILayout.Button("Revert"))
                {
                    Script.Revert();
                }
            }
            else
            {
                if (GUILayout.Button("Apply"))
                {
                    Script.Apply();
                }
            }
        }
    }
}
