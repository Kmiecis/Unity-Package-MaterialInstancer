using Common.Materials;
using UnityEditor;
using UnityEngine;

namespace CommonEditor.Materials
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(MaterialAlpha))]
    public class MaterialAlphaEditor : MaterialPropertyNamedEditor
    {
        protected override void CustomDrawField(Rect rect, SerializedProperty property)
        {
            UEditorGUI.PropertySliderField(rect, property, 0.0f, 1.0f);
        }
    }
}