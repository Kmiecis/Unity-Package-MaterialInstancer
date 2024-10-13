using UnityEditor;
using UnityEngine;

namespace CommonEditor.Materials
{
    internal static class UEditorGUI
    {
        public static Color PropertyColorFieldHDR(Rect rect, GUIContent label, SerializedProperty property)
        {
            var result = EditorGUI.ColorField(rect, label, property.colorValue, true, true, true);
            property.colorValue = result;
            return result;
        }

        public static float PropertyFloatField(Rect rect, GUIContent label, SerializedProperty property)
        {
            var result = EditorGUI.FloatField(rect, label, property.floatValue);
            property.floatValue = result;
            return result;
        }

        public static float PropertySliderField(Rect rect, SerializedProperty property, float leftValue, float rightValue)
        {
            var result = EditorGUI.Slider(rect, property.floatValue, leftValue, rightValue);
            property.floatValue = result;
            return result;
        }

        public static bool PropertyToggle(Rect rect, SerializedProperty property)
        {
            var result = EditorGUI.Toggle(rect, property.boolValue);
            property.boolValue = result;
            return result;
        }

        public static string PropertyPopup(Rect position, SerializedProperty property, string[] displayedOptions)
        {
            var index = displayedOptions.IndexOfOrDefault(property.stringValue);

            var result = Popup(position, ref index, displayedOptions);
            property.stringValue = result;
            return result;
        }

        public static string Popup(Rect position, ref int selectedIndex, string[] displayedOptions)
        {
            selectedIndex = EditorGUI.Popup(position, selectedIndex, displayedOptions);
            return displayedOptions[selectedIndex];
        }
    }
}