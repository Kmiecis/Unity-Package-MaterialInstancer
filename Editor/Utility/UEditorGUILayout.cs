using UnityEditor;

namespace CommonEditor.Materials
{
    public static class UEditorGUILayout
    {
        public static string Popup(ref int selectedIndex, string[] displayedOptions)
        {
            selectedIndex = EditorGUILayout.Popup(selectedIndex, displayedOptions);
            if (selectedIndex < displayedOptions.Length)
            {
                return displayedOptions[selectedIndex];
            }
            return null;
        }

        public static void PropertyPopup(SerializedProperty property, ref int selectedIndex, string[] displayedOptions)
        {
            var option = Popup(ref selectedIndex, displayedOptions);
            property.stringValue = option;
        }
    }
}