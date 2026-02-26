using Common.Materials;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace CommonEditor.Materials
{
    [CustomEditor(typeof(MaterialPropertyNamed<>), true)]
    public class MaterialPropertyNamedEditor : Editor
    {
        protected static readonly GUIContent EmptyLabel = new GUIContent(" ");

        private static float LabelWidth => EditorGUIUtility.labelWidth;
        private static float FieldOffset => LabelWidth + UEditorGUIUtility.SpaceWidth;

        private static HashSet<string> _NamesCache;

        private SerializedProperty _instancesProperty;
        private SerializedProperty _nameProperty;
        private SerializedProperty _valueProperty;

        private IMaterialPropertyNamedVerifier Script => (IMaterialPropertyNamedVerifier)target;

        static MaterialPropertyNamedEditor()
        {
            _NamesCache = new HashSet<string>();
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(_instancesProperty);

            var names = FindPropertyNames();
            if (names != null)
            {
                var rect = EditorGUILayout.GetControlRect();
                DrawNames(rect, _nameProperty, names);
                DrawField(rect, _valueProperty);
            }
            else
            {
                EditorGUILayout.HelpBox($"Unable to find matching properties for type '{_valueProperty.type}'", MessageType.Warning);
            }

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawNames(Rect rect, SerializedProperty property, string[] names)
        {
            rect.width = LabelWidth;
            UEditorGUI.PropertyPopup(rect, property, names);
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
            EditorGUI.PropertyField(rect, property, EmptyLabel, true);
        }

        private string[] FindPropertyNames()
        {
            _NamesCache.Clear();

            foreach (var material in GetMaterials())
            {
                var properties = MaterialEditor.GetMaterialProperties(new Material[] { material });
                foreach (var property in properties)
                {
                    if (Script.CanHandleProperty(material, property.name))
                    {
                        _NamesCache.Add(property.name);
                    }
                }
            }

            if (_NamesCache.Count == 0)
                return null;

            return _NamesCache.ToArray();
        }

        private IEnumerable<Material> GetMaterials()
        {
            var instances = (List<MaterialInstance>)_instancesProperty.GetValue();
            if (instances != null && instances.Count > 0)
            {
                foreach (var instance in instances)
                {
                    if (instance != null)
                    {
                        var material = instance.Source;
                        if (material != null)
                        {
                            yield return material;
                        }
                    }
                }
            }
        }

        private void OnEnable()
        {
            _instancesProperty = serializedObject.FindProperty("_instances");
            _nameProperty = serializedObject.FindProperty("_name");
            _valueProperty = serializedObject.FindProperty("_value");
        }
    }
}