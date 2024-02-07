using Common.Materials;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEditor.MaterialProperty;

namespace CommonEditor.Materials
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(MaterialPropertyNamed<>), true)]
    public class MaterialPropertyNamedEditor : Editor
    {
        private SerializedProperty _instancesProperty;
        private SerializedProperty _nameProperty;
        private SerializedProperty _activeProperty;
        private SerializedProperty _valueProperty;

        private int _nameIndex;

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(_instancesProperty);

            var names = FindPropertyNames(_valueProperty.type);
            if (names.Length > 0)
            {
                EditorGUILayout.BeginHorizontal();

                UEditorGUILayout.PropertyToggle(_activeProperty, GUILayout.ExpandWidth(false), GUILayout.MaxWidth(15.0f));

                UEditorGUILayout.PropertyPopup(_nameProperty, ref _nameIndex, names);

                EditorGUILayout.EndHorizontal();

                DrawPropertyField(_valueProperty);
            }
            else
            {
                if (_instancesProperty.arraySize > 0)
                {
                    EditorGUILayout.HelpBox($"Unable to find matching properties for type '{_valueProperty.type}'", MessageType.Warning);
                }
            }

            serializedObject.ApplyModifiedProperties();
        }

        private void OnEnable()
        {
            _instancesProperty = serializedObject.FindProperty("_instances");
            _nameProperty = serializedObject.FindProperty("_name");
            _activeProperty = serializedObject.FindProperty("_active");
            _valueProperty = serializedObject.FindProperty("_value");
        }

        private void DrawPropertyField(SerializedProperty property)
        {
            if (IsColor(property.type))
            {
                property.colorValue = EditorGUILayout.ColorField(new GUIContent(property.displayName), property.colorValue, true, true, true);
            }
            else
            {
                EditorGUILayout.PropertyField(property);
            }
        }

        private string[] FindPropertyNames(string valueType)
        {
            var result = new List<string>();

            foreach (var instanceProperty in _instancesProperty.GetArrayElements())
            {
                var instance = (MaterialInstance)instanceProperty.objectReferenceValue;
                if (instance != null)
                {
                    var material = instance.Original;

                    var properties = MaterialEditor.GetMaterialProperties(new Material[] { material });
                    foreach (var property in properties)
                    {
                        if (IsMatchingType(property.type, valueType))
                        {
                            result.Add(property.name);
                        }
                    }
                }
            }

            return result.ToArray();
        }

        private bool IsMatchingType(PropType propertyType, string valueType)
        {
            switch (propertyType)
            {
                case PropType.Color: return IsColor(valueType);
                case PropType.Vector: return IsVector(valueType);
                case PropType.Float:
                case PropType.Range: return IsFloat(valueType);
                case PropType.Texture: return IsTexture(valueType);
                case PropType.Int: return IsInt(valueType);
            }
            return false;
        }

        private bool IsColor(string valueType)
        {
            return valueType == "Color";
        }

        private bool IsVector(string valueType)
        {
            return (
                valueType == "Vector2" ||
                valueType == "Vector3" ||
                valueType == "Vector4"
            );
        }

        private bool IsFloat(string valueType)
        {
            return valueType == "float";
        }

        private bool IsTexture(string valueType)
        {
            return (
                valueType.Contains("Texture") ||
                valueType == "Vector2" // Texture Scale and Offset
            );
        }

        private bool IsInt(string valueType)
        {
            return valueType == "int";
        }
    }
}