using Common.Materials;
using UnityEditor;
using UnityEngine;

namespace CommonEditor.Materials
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(MaterialVector4))]
    public class MaterialVector4Editor : MaterialPropertyNamedEditor
    {
        private const float LabelWdith = 15.0f;

        protected override void CustomDrawField(Rect rect, SerializedProperty property)
        {
            var fieldWidth = rect.width * 0.25f;
            foreach (SerializedProperty child in property.Copy())
            {
                rect.width = LabelWdith;
                EditorGUI.LabelField(rect, child.displayName);

                rect.x += rect.width;
                rect.width = fieldWidth - rect.width - 3.0f;
                UEditorGUI.PropertyFloatField(rect, child);

                rect.x += rect.width + 4.0f;
            }
        }
    }
}