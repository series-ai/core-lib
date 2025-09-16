using System;
using UnityEditor;
using UnityEngine;

namespace Padoru.Core.Editor
{
	public class KeyValueDrawer
	{
		private const float BOX_BORDER = 8;
		private const float KEY_AND_VALUE_SPACING = 10;
		private const float KEY_WIDTH_MULTIPLIER = 0.3f;
		private const float LINE_WIDTH = 1;
		private const float MINUS_BUTTON_WIDTH = 30;
		private const float MINUS_BUTTON_HEIGHT = 15;

		private Color boxColor = new Color(0.2f, 0.2f, 0.2f);
		private Color lineColor = new Color(0.15f, 0.15f, 0.15f);

		public float KeyValueHeight { get; private set; }

		public KeyValueDrawer(float highestPropertyHeight)
		{
			KeyValueHeight = highestPropertyHeight;
			KeyValueHeight += BOX_BORDER * 2;
			KeyValueHeight += MINUS_BUTTON_HEIGHT;
		}

		public void Draw(Rect keyValueRect, SerializedProperty key, SerializedProperty value, Action onRemoveButtonClick)
		{
			DrawBoxAndLine(keyValueRect);

			DrawFields(keyValueRect, key, value);

			DrawRemoveButton(keyValueRect, onRemoveButtonClick);
		}

		private void DrawBoxAndLine(Rect keyValueRect)
		{
			var boxRect = GetBoxRect(keyValueRect);
			var lineRect = GetLineRect(keyValueRect);

			EditorGUI.DrawRect(boxRect, boxColor);
			EditorGUI.DrawRect(lineRect, lineColor);
		}

		private void DrawFields(Rect keyValueRect, SerializedProperty key, SerializedProperty value)
		{
			var keyRect = GetKeyRect(keyValueRect);
			var valueRect = GetValueRect(keyValueRect);

			FieldDrawer.DrawField(key, keyRect);
			FieldDrawer.DrawField(value, valueRect);
		}

		private void DrawRemoveButton(Rect keyValueRect, Action onRemoveButtonClick)
		{
			var style = GUIStyles.ListButtonStyle;
			var minusIcon = UnityIcons.GetMinusIcon();

			var buttonBoxRect = GetMinusButtonBoxRect(keyValueRect);
			var buttonRect = GetMinusButtonRect(buttonBoxRect, style, minusIcon);

			EditorGUI.DrawRect(buttonBoxRect, boxColor);
			if (GUI.Button(buttonRect, minusIcon, style))
			{
				onRemoveButtonClick?.Invoke();
			}
		}

		private Rect GetBoxRect(Rect keyValueRect)
		{
			return new Rect(keyValueRect.x,
							keyValueRect.y,
							keyValueRect.width,
							KeyValueHeight - MINUS_BUTTON_HEIGHT);
		}

		private Rect GetLineRect(Rect keyValueRect)
		{
			return new Rect(keyValueRect.x + (keyValueRect.width * KEY_WIDTH_MULTIPLIER) - LINE_WIDTH / 2f + BOX_BORDER, 
							keyValueRect.y + BOX_BORDER,
							LINE_WIDTH,
							KeyValueHeight - BOX_BORDER * 2f - MINUS_BUTTON_HEIGHT);
		}

		private Rect GetKeyRect(Rect keyValueRect)
		{
			return new Rect(keyValueRect.x + BOX_BORDER, 
							keyValueRect.y + BOX_BORDER, 
							keyValueRect.width * KEY_WIDTH_MULTIPLIER - KEY_AND_VALUE_SPACING, 
							keyValueRect.height);
		}

		private Rect GetValueRect(Rect keyValueRect)
		{
			return new Rect(keyValueRect.x + (keyValueRect.width * KEY_WIDTH_MULTIPLIER) + BOX_BORDER + KEY_AND_VALUE_SPACING * 2f, // <-- Spacing multiplied by 2 because of the folding arrow. Remove if able to remove the folding arrow
							keyValueRect.y + BOX_BORDER, 
							keyValueRect.width * (1 - KEY_WIDTH_MULTIPLIER) - BOX_BORDER * 2f - KEY_AND_VALUE_SPACING * 2f, // <-- Same here
							keyValueRect.height);
		}

		private Rect GetMinusButtonBoxRect(Rect keyValueRect)
		{
			return new Rect(keyValueRect.x + keyValueRect.width - MINUS_BUTTON_WIDTH,
							keyValueRect.y + KeyValueHeight - MINUS_BUTTON_HEIGHT,
							MINUS_BUTTON_WIDTH,
							MINUS_BUTTON_HEIGHT);
		}

		private Rect GetMinusButtonRect(Rect buttonBoxRect, GUIStyle style, GUIContent minusIcon)
		{
			var size = style.CalcSize(minusIcon);

			return new Rect(buttonBoxRect.x + buttonBoxRect.width / 2f - size.x / 2f,
							buttonBoxRect.y,
							size.x,
							size.y);
		}
	}
}
