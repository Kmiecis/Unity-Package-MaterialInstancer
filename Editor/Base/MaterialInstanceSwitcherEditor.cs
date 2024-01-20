using Common.Materials;
using UnityEditor;
using UnityEngine;

namespace CommonEditor.Materials
{
    [CustomEditor(typeof(MaterialInstanceSwitcher))]
    public class MaterialInstanceSwitcherEditor : Editor
    {
        private MaterialInstanceSwitcher Script
        {
            get => (MaterialInstanceSwitcher)target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (Script.IsSwitched)
            {
                if (GUILayout.Button(nameof(Script.SetOriginal)))
                {
                    Script.SetOriginal();
                }
            }
            else
            {
                if (GUILayout.Button(nameof(Script.SetSwitched)))
                {
                    Script.SetSwitched();
                }
            }
        }
    }
}
