using Common.Materials;
using UnityEditor;
using UnityEngine;

namespace CommonEditor.Materials
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(MaterialProperty<>), true)]
    public class MaterialPropertyEditor : Editor
    {
        protected static readonly GUIContent EmptyLabel = new GUIContent(" ");

        private static float LabelWidth => EditorGUIUtility.labelWidth;
        private static float FieldOffset => LabelWidth + UEditorGUIUtility.SpaceWidth;

        private SerializedProperty _instancesProperty;
        private SerializedProperty _valueProperty;

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(_instancesProperty);

            var rect = EditorGUILayout.GetControlRect();
            DrawLabel(rect, _valueProperty);
            DrawField(rect, _valueProperty);

            serializedObject.ApplyModifiedProperties();
        }
        
        private void DrawLabel(Rect rect, SerializedProperty property)
        {
            rect.width = LabelWidth;
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
            using var labelWidthScope = new UEditorGUIUtility.LabelWidthScope(UEditorGUIUtility.IndentWidth);
            EditorGUI.PropertyField(rect, property, EmptyLabel);
        }

        private void OnEnable()
        {
            _instancesProperty = serializedObject.FindProperty("_instances");
            _valueProperty = serializedObject.FindProperty("_value");
        }
    }
}