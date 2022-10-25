using Common.Materializer;
using UnityEditor;
using UnityEngine;

namespace CommonEditor.Materializer
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
