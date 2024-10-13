using System;
using UnityEditor;

namespace CommonEditor.Materials
{
    public static class UEditorGUIUtility
    {
        public const float SpaceWidth = 2.0f;
        public const float IndentWidth = 10.0f;
        public const float LabelWidth = 12.0f;

        public class LabelWidthScope : IDisposable
        {
            private readonly float _labelWidth;

            public float LabelWidth
            {
                set => EditorGUIUtility.labelWidth = value;
            }

            public LabelWidthScope()
            {
                _labelWidth = EditorGUIUtility.labelWidth;
            }

            public LabelWidthScope(float labelWidth) : this()
            {
                LabelWidth = labelWidth;
            }

            public void Dispose()
            {
                EditorGUIUtility.labelWidth = _labelWidth;
            }
        }
    }
}