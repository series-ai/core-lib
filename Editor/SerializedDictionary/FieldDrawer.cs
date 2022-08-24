using UnityEditor;
using UnityEngine;

namespace Padoru.Core.Editor
{
	public static class FieldDrawer
	{
		private const float LABEL_WIDTH_MULTIPLIER = 0.4f;

		public static void DrawField(SerializedProperty property, Rect position)
		{
			var propType = property.type;

			if (propType.Equals("string"))
			{
				DrawString(property, position);
			}
			else
			{
				DrawPropertyField(position, property);
			}
		}

		public static void DrawString(SerializedProperty property, Rect position)
		{
			property.stringValue = EditorGUI.TextField(position, property.stringValue);
		}

		public static void DrawPropertyField(Rect position, SerializedProperty property)
		{
			var labelWidth = EditorGUIUtility.labelWidth;

			property.isExpanded = true;

			// TODO: remove folding arrow and indentation
			// Checkout -> https://github.com/starikcetin/Unity-SerializableDictionary/blob/master/Assets/SerializableDictionary/Editor/SerializableDictionaryPropertyDrawer.cs

			//EditorGUI.indentLevel--;

			EditorGUIUtility.labelWidth = position.width * LABEL_WIDTH_MULTIPLIER;

			var guiContent = new GUIContent(property.type);

			EditorGUI.PropertyField(position, property, guiContent, true);

			//EditorGUI.indentLevel++;

			EditorGUIUtility.labelWidth = labelWidth;
		}
	}
}
