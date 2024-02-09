using Common.Materials;
using UnityEditor;
using UnityEngine;

namespace CommonEditor.Materials
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(MaterialProperty<>), true)]
    public class MaterialPropertyEditor : Editor
    {
        private const float SpaceWidth = 2.0f;

        protected static readonly GUIContent EmptyLabel = new GUIContent(string.Empty);

        private static float ToggleSize => EditorGUIUtility.singleLineHeight;
        private static float LabelOffset => ToggleSize + SpaceWidth;
        private static float LabelWidth => EditorGUIUtility.labelWidth;
        private static float FieldOffset => LabelWidth + SpaceWidth;

        private SerializedProperty _instanceProperty;
        private SerializedProperty _activeProperty;
        private SerializedProperty _valueProperty;

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(_instanceProperty);

            var rect = EditorGUILayout.GetControlRect();
            DrawToggle(rect, _activeProperty);
            DrawLabel(rect, _valueProperty);
            DrawField(rect, _valueProperty);

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawToggle(Rect rect, SerializedProperty property)
        {
            rect.width = ToggleSize;
            rect.height = ToggleSize;
            UEditorGUI.PropertyToggle(rect, property);
        }

        private void DrawLabel(Rect rect, SerializedProperty property)
        {
            rect.x += LabelOffset;
            rect.width = LabelWidth - LabelOffset;
            EditorGUI.LabelField(rect, property.displayName);
        }

        private void DrawField(Rect rect, SerializedProperty property)
        {
            rect.x += FieldOffset;
            rect.width -= FieldOffset;
            CustomDrawField(rect, property);
        }

        protected virtual void CustomDrawField(Rect rect, SerializedProperty property)
        {
            EditorGUI.PropertyField(rect, property, EmptyLabel);
        }

        private void OnEnable()
        {
            _instanceProperty = serializedObject.FindProperty("_instance");
            _activeProperty = serializedObject.FindProperty("_active");
            _valueProperty = serializedObject.FindProperty("_value");
        }
    }
}