using System.Collections;
using UnityEditor;
using UnityEngine;

namespace Padoru.Core.Editor
{
    public abstract class SerializedDictionaryDrawer<T> : PropertyDrawer where T : class, IDictionary
	{
		private const int ADD_BUTTON_WIDTH = 20;
		private const int VERTICAL_SPACING = 10;

		private bool show = true;
		private int itemsCount;
		private bool initialized;

		private SerializedProperty keysProperty;
		private SerializedProperty valuesProperty;
		private SerializedProperty addedValueThroughEditorProperty;
		private SerializedObject serializedObject;

		private void Initialize(SerializedProperty property)
		{
			serializedObject = property.serializedObject;

			keysProperty = property.FindPropertyRelative("keys");
			valuesProperty = property.FindPropertyRelative("values");
			addedValueThroughEditorProperty = property.FindPropertyRelative("addedItemThroughEditor");

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

			var titlePosition = new Rect(position.x, position.y, position.width - ADD_BUTTON_WIDTH, EditorGUIUtility.singleLineHeight);
			var addButtonPosition = new Rect(position.width - ADD_BUTTON_WIDTH, position.y, ADD_BUTTON_WIDTH, EditorGUIUtility.singleLineHeight);
			
			show = EditorGUI.Foldout(titlePosition, show, label, false, GUIStyles.FoldableTitle);
			if(GUI.Button(addButtonPosition, "+"))
			{
				OnAddButtonClick();
			}

			if (show)
			{
				DrawKeyAndValues(position);
			}

			EditorGUI.EndProperty();
		}

		private void OnAddButtonClick()
		{
			serializedObject.Update();

			addedValueThroughEditorProperty.boolValue = true;

			keysProperty.arraySize++;
			valuesProperty.arraySize++;
			itemsCount++;

			serializedObject.ApplyModifiedProperties();
		}

		private void DrawKeyAndValues(Rect position)
		{
			var keyValueRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
			var yOffset = keyValueRect.y + VERTICAL_SPACING;

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
			var titleHeight = EditorGUIUtility.singleLineHeight;

			if (keysProperty == null || keysProperty.arraySize <= 0 || 
				valuesProperty == null || valuesProperty.arraySize <= 0 || 
				!show)
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
