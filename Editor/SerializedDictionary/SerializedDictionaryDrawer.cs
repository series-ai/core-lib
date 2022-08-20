using System.Collections;
using UnityEditor;
using UnityEngine;

namespace Padoru.Core.Editor
{
    public class SerializedDictionaryDrawer<T> : PropertyDrawer where T : IDictionary
	{
		private const int ADD_BUTTON_WIDTH = 20;
		private const int VERTICAL_SPACING = 10;

		private bool show = true;
		private int itemsCount;
		private bool initialized;

		private void Initialize(SerializedProperty property)
		{
			var keysProperty = property.FindPropertyRelative("keys");

			itemsCount = keysProperty.arraySize;
		}

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			if (!initialized)
			{
				initialized = true;
				Initialize(property);
			}

			EditorGUI.BeginProperty(position, label, property);

			var keysProperty = property.FindPropertyRelative("keys");
			var valuesProperty = property.FindPropertyRelative("values");

			var titlePosition = new Rect(position.x, position.y, position.width - ADD_BUTTON_WIDTH, EditorGUIUtility.singleLineHeight);
			var addButtonPosition = new Rect(position.width - ADD_BUTTON_WIDTH, position.y, ADD_BUTTON_WIDTH, EditorGUIUtility.singleLineHeight);
			
			show = EditorGUI.Foldout(titlePosition, show, label, false, GUIStyles.FoldableTitle);
			if(GUI.Button(addButtonPosition, "+"))
			{
				OnAddButtonClick(keysProperty, valuesProperty);
			}

			if (show)
			{
				DrawKeyAndValues(position, property, keysProperty, valuesProperty);
			}

			EditorGUI.EndProperty();
		}

		private void OnAddButtonClick(SerializedProperty keysProperty, SerializedProperty valuesProperty)
		{
			keysProperty.arraySize++;
			valuesProperty.arraySize++;
			itemsCount++;
		}

		private void DrawKeyAndValues(Rect position, SerializedProperty dictionaryProperty, SerializedProperty keysProperty, SerializedProperty valuesProperty)
		{
			var keyValueRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
			var yOffset = keyValueRect.y + VERTICAL_SPACING;

			var serializedObject = dictionaryProperty.serializedObject;
			serializedObject.Update();

			for (int i = 0; i < keysProperty.arraySize; i++)
			{
				keyValueRect.y = position.y + yOffset * (i + 1);

				var key = keysProperty.GetArrayElementAtIndex(i);
				var value = valuesProperty.GetArrayElementAtIndex(i);

				KeyValueDrawer.Draw(keyValueRect, key, value);
			}

			serializedObject.ApplyModifiedProperties();
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			var keysProperty = property.FindPropertyRelative("keys");
			var valuesProperty = property.FindPropertyRelative("values");
			var titleHeight = EditorGUIUtility.singleLineHeight;

			if (keysProperty.arraySize <= 0 || valuesProperty.arraySize <= 0 || !show)
			{
				return titleHeight;
			}

			var firstKey = keysProperty.GetArrayElementAtIndex(0);
			var firstValue = valuesProperty.GetArrayElementAtIndex(0);

			var highestPropertyHeight = Mathf.Max(EditorGUI.GetPropertyHeight(firstKey),
										EditorGUI.GetPropertyHeight(firstValue));
			var propertiesHeight = (highestPropertyHeight + KeyValueDrawer.BOX_BORDER * 2) * itemsCount;

			var height = titleHeight + VERTICAL_SPACING + propertiesHeight + EditorGUIUtility.singleLineHeight;

			return height;
		}
	}
}
