using Common.Materials;
using UnityEditor;
using UnityEngine;

namespace CommonEditor.Materials
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(MaterialColor))]
    public class MaterialColorEditor : MaterialPropertyNamedEditor
    {
        protected override void CustomDrawField(Rect rect, SerializedProperty property)
        {
            UEditorGUI.PropertyColorFieldHDR(rect, EmptyLabel, property);
        }
    }
}