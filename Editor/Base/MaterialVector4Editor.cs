using Common.Materials;
using UnityEditor;
using UnityEngine;

namespace CommonEditor.Materials
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(MaterialVector4))]
    public class MaterialVector4Editor : MaterialPropertyNamedEditor
    {
        protected override void CustomDrawField(Rect rect, SerializedProperty property)
        {
            using var labelWidthScope = new UEditorGUIUtility.LabelWidthScope(UEditorGUIUtility.IndentWidth);

            rect.x += UEditorGUIUtility.IndentWidth;
            rect.width -= UEditorGUIUtility.IndentWidth + 3 * UEditorGUIUtility.SpaceWidth;

            rect.width = rect.width * 0.25f;
            foreach (SerializedProperty child in property.Copy())
            {
                var label = new GUIContent(child.displayName);

                UEditorGUI.PropertyFloatField(rect, label, child);

                rect.x += rect.width + UEditorGUIUtility.SpaceWidth;
            }
        }
    }
}