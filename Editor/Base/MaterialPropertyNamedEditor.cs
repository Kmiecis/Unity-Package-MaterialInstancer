using Common.Materials;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace CommonEditor.Materials
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(MaterialPropertyNamed<>), true)]
    public class MaterialPropertyNamedEditor : Editor
    {
        protected static readonly GUIContent EmptyLabel = new GUIContent(" ");

        private static float ToggleSize => EditorGUIUtility.singleLineHeight;
        private static float LabelOffset => ToggleSize + UEditorGUIUtility.SpaceWidth;
        private static float LabelWidth => EditorGUIUtility.labelWidth;
        private static float FieldOffset => LabelWidth + UEditorGUIUtility.SpaceWidth;

        private SerializedProperty _instancesProperty;
        private SerializedProperty _nameProperty;
        private SerializedProperty _activeProperty;
        private SerializedProperty _valueProperty;

        private IMaterialPropertyNamedVerifier Script => (IMaterialPropertyNamedVerifier)target;

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(_instancesProperty);

            var names = FindPropertyNames();
            if (names != null)
            {
                var rect = EditorGUILayout.GetControlRect();
                DrawToggle(rect, _activeProperty);
                DrawNames(rect, _nameProperty, names);
                DrawField(rect, _valueProperty);
            }
            else
            {
                EditorGUILayout.HelpBox($"Unable to find matching properties for type '{_valueProperty.type}'", MessageType.Warning);
            }

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawToggle(Rect rect, SerializedProperty property)
        {
            rect.width = ToggleSize;
            rect.height = ToggleSize;
            UEditorGUI.PropertyToggle(rect, property);
        }

        private void DrawNames(Rect rect, SerializedProperty property, string[] names)
        {
            rect.x += LabelOffset;
            rect.width = LabelWidth - LabelOffset;
            UEditorGUI.PropertyPopup(rect, property, names);
        }

        private void DrawField(Rect rect, SerializedProperty property)
        {
            rect.x += FieldOffset;
            rect.width -= FieldOffset;

            using var labelWidthScope = new UEditorGUIUtility.LabelWidthScope(UEditorGUIUtility.IndentWidth);
            CustomDrawField(rect, property);
        }

        protected virtual void CustomDrawField(Rect rect, SerializedProperty property)
        {
            EditorGUI.PropertyField(rect, property, EmptyLabel, true);
        }

        private string[] FindPropertyNames()
        {
            HashSet<string> result = null;

            var instances = (MaterialInstance[])_instancesProperty.GetValue();
            if (instances != null && instances.Length > 0)
            {
                foreach (var instance in instances)
                {
                    var material = instance.Source;
                    if (material != null)
                    {
                        var names = new HashSet<string>();

                        var properties = MaterialEditor.GetMaterialProperties(new Material[] { material });
                        foreach (var property in properties)
                        {
                            if (Script.CanHandleProperty(material, property.name))
                            {
                                names.Add(property.name);
                            }
                        }

                        if (result == null)
                        {
                            result = names;
                        }
                        else
                        {
                            result.IntersectWith(names);
                        }
                    }
                }
            }

            if (result == null || result.Count == 0)
                return null;

            return result.ToArray();
        }

        private void OnEnable()
        {
            _instancesProperty = serializedObject.FindProperty("_instances");
            _activeProperty = serializedObject.FindProperty("_active");
            _nameProperty = serializedObject.FindProperty("_name");
            _valueProperty = serializedObject.FindProperty("_value");
        }
    }
}