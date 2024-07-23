using UnityEditor;
using UnityEngine;

namespace Padoru.Core.Editor
{
    [CustomPropertyDrawer(typeof(SerializedDictionary<,>))]
    public class SerializedDictionaryDrawer : PropertyDrawer
    {
        private const int ADD_BUTTON_WIDTH = 20;
        private const int VERTICAL_SPACING = 5;

        private bool show = true;
        private int itemsCount;
        private bool initialized;
        private bool shouldCheckIfValueWasAdded;

        private SerializedProperty keysProperty;
        private SerializedProperty valuesProperty;
        private SerializedProperty addedValueThroughEditorProperty;
        private SerializedObject serializedObject;
        private KeyValueDrawer keyValueDrawer;

        private void Initialize(SerializedProperty property)
        {
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

            if (shouldCheckIfValueWasAdded)
            {
                shouldCheckIfValueWasAdded = false;
                if (itemsCount > keysProperty.arraySize)
                {
                    itemsCount = keysProperty.arraySize;
                }
            }

            EditorGUI.BeginProperty(position, label, property);

            var titlePosition = new Rect(position.x, position.y, position.width - ADD_BUTTON_WIDTH, EditorGUIUtility.singleLineHeight);
            var addButtonPosition = new Rect(position.width - ADD_BUTTON_WIDTH, position.y, ADD_BUTTON_WIDTH, EditorGUIUtility.singleLineHeight);

            // Add the title height and some spacing
            position.y += EditorGUIUtility.singleLineHeight + VERTICAL_SPACING;

            show = EditorGUI.Foldout(titlePosition, show, label, false, GUIStyles.FoldableTitle);

            if (GUI.Button(addButtonPosition, UnityIcons.GetPlusIcon(), GUIStyles.ListButtonStyle))
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
            addedValueThroughEditorProperty.boolValue = true;

            keysProperty.arraySize++;
            valuesProperty.arraySize++;
            itemsCount++;

            shouldCheckIfValueWasAdded = true;
        }

        private void DrawKeyAndValues(Rect position)
        {
            if (keysProperty.arraySize <= 0)
            {
                return;
            }

            if (keyValueDrawer == null)
            {
                CreateKeyValueDrawer();
            }

            var keyValueRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            var yOffset = keyValueDrawer.KeyValueHeight + VERTICAL_SPACING;

            for (int i = 0; i < keysProperty.arraySize; i++)
            {
                var key = keysProperty.GetArrayElementAtIndex(i);
                var value = valuesProperty.GetArrayElementAtIndex(i);

                keyValueDrawer.Draw(keyValueRect, key, value, () => RemoveElement(i));

                keyValueRect.y += yOffset;
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var titleHeight = EditorGUIUtility.singleLineHeight + VERTICAL_SPACING;

            if (keyValueDrawer == null || !show)
            {
                return titleHeight;
            }

            var propertiesHeight = (keyValueDrawer.KeyValueHeight + VERTICAL_SPACING) * itemsCount;

            var height = titleHeight + propertiesHeight;

            return height;
        }

        private void CreateKeyValueDrawer()
        {
            if (keysProperty.arraySize <= 0)
            {
                Debug.LogError($"Cannot create {typeof(KeyValueDrawer)} because there are no elements in the array so the height cannot be calculated");
                return;
            }

            var firstKey = keysProperty.GetArrayElementAtIndex(0);
            var firstValue = valuesProperty.GetArrayElementAtIndex(0);

            var highestPropertyHeight = Mathf.Max(EditorGUI.GetPropertyHeight(firstKey),
                                        EditorGUI.GetPropertyHeight(firstValue));

            keyValueDrawer = new KeyValueDrawer(highestPropertyHeight);
        }

        private void RemoveElement(int index)
        {
            keysProperty.DeleteArrayElementAtIndex(index);
            valuesProperty.DeleteArrayElementAtIndex(index);

            itemsCount--;
        }
    }
}