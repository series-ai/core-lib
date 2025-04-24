using Padoru.Core.Utils;
using UnityEditor;
using UnityEngine;

namespace Padoru.Core.Editor
{
    [CustomPropertyDrawer(typeof(MinMaxFloat))]
    public class MinMaxFloatDrawer : PropertyDrawer
    {
        private const int SPACING = 40;
        private const int LABEL_WIDTH = 30;
        private const string MIN_FIELD_NAME = "Min";
        private const string MAX_FIELD_NAME = "Max";
		
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUILayout.LabelField(label.text);
			
            EditorGUILayout.BeginHorizontal();

            var fieldWidth = (EditorGUIUtility.currentViewWidth - SPACING - LABEL_WIDTH * 2) / 2;

            EditorGUILayout.LabelField(MIN_FIELD_NAME, GUILayout.Width(LABEL_WIDTH));
            EditorGUILayout.PropertyField(property.FindPropertyRelative(MIN_FIELD_NAME), GUIContent.none, GUILayout.Width(fieldWidth));
            EditorGUILayout.LabelField(MAX_FIELD_NAME, GUILayout.Width(LABEL_WIDTH));
            EditorGUILayout.PropertyField(property.FindPropertyRelative(MAX_FIELD_NAME), GUIContent.none, GUILayout.Width(fieldWidth));
			
            EditorGUILayout.EndHorizontal();
        }
    }
}