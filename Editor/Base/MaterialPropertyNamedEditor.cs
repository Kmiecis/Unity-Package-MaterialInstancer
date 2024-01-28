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
            EditorGUILayout.PropertyField(_activeProperty);

            var names = FindPropertyNames();
            if (names.Length > 0)
            {
                EditorGUILayout.PropertyField(_valueProperty);
                UEditorGUILayout.PropertyPopup(_nameProperty, ref _nameIndex, names);
            }
            else
            {
                var valueType = ValueType();
                EditorGUILayout.HelpBox($"Unable to find matching properties for type '{valueType}'", MessageType.Warning);
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

        private string ValueType()
        {
            return _valueProperty.type;
        }

        private string[] FindPropertyNames()
        {
            var result = new List<string>();

            var valueType = ValueType();

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