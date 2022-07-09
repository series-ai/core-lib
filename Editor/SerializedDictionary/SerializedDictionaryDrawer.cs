using System.Collections;
using UnityEditor;
using UnityEngine;
using Debug = Padoru.Diagnostics.Debug;

namespace Padoru.Core.Editor
{
	public class SerializedDictionaryDrawer<T> : PropertyDrawer where T : IDictionary
	{
		private const int ADD_BUTTON_WIDTH = 20;
		private const int SPACING = 10;

		private bool show;

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			EditorGUI.BeginProperty(position, label, property);

			var keysProperty = property.FindPropertyRelative("keys");
			var valuesProperty = property.FindPropertyRelative("values");

			var titlePosition = new Rect(position.x, position.y, position.width - ADD_BUTTON_WIDTH, position.height);
			var addButtonPosition = new Rect(position.width - ADD_BUTTON_WIDTH, position.y, ADD_BUTTON_WIDTH, position.height);
			
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
		}

		private void DrawKeyAndValues(Rect position, SerializedProperty dictionaryProperty, SerializedProperty keysProperty, SerializedProperty valuesProperty)
		{
			// Definir ancho de key y value

			var keyValueRect = new Rect(position);
			var yOffset = position.height + SPACING;

			var serializedObject = dictionaryProperty.serializedObject;
			serializedObject.Update();

			for (int i = 0; i < keysProperty.arraySize; i++)
			{
				keyValueRect.y = position.y + yOffset * (i + 1);
				var element = keysProperty.GetArrayElementAtIndex(i);

				EditorGUI.PropertyField(keyValueRect, element, true);


				// Obtener position del key
				// Obtener position del value
				// Key y value height?


				// Dibujar el key
				// Dibujar el value


				// offsetear la posicion apra el siguiente elemento
			}
			serializedObject.ApplyModifiedProperties();
		}
	}
}
