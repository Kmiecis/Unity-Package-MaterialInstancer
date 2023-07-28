using Common.Materials;
using UnityEditor;
using UnityEngine;

namespace CommonEditor.Materials
{
    [CustomEditor(typeof(MaterialSwitcher))]
    public class MaterialSwitcherEditor : Editor
    {
        private MaterialSwitcher Script
        {
            get => (MaterialSwitcher)target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (Script.IsSwitched)
            {
                if (GUILayout.Button(nameof(Script.Revert)))
                {
                    Script.Revert();
                }
            }
            else
            {
                if (GUILayout.Button(nameof(Script.Switch)))
                {
                    Script.Switch();
                }
            }
        }
    }
}
