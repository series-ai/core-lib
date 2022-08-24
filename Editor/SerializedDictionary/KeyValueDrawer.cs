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

		private Color boxColor = new Color(0.2f, 0.2f, 0.2f);
		private Color lineColor = new Color(0.15f, 0.15f, 0.15f);

		public float KeyValueHeight { get; private set; }

		public KeyValueDrawer(float highestPropertyHeight)
		{
			KeyValueHeight = highestPropertyHeight;
			KeyValueHeight += BOX_BORDER * 2;
		}

		public void Draw(Rect keyValueRect, SerializedProperty key, SerializedProperty value)
		{
			DrawBoxAndLine(keyValueRect, KeyValueHeight);

			DrawFields(keyValueRect, key, value);
		}

		private void DrawBoxAndLine(Rect keyValueRect, float boxHeight)
		{
			var boxRect = GetBoxRect(keyValueRect, boxHeight);
			var lineRect = GetLineRect(keyValueRect, boxHeight);

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

		private Rect GetBoxRect(Rect keyValueRect, float boxHeight)
		{
			return new Rect(keyValueRect.x,
							keyValueRect.y,
							keyValueRect.width,
							boxHeight);
		}

		private Rect GetLineRect(Rect keyValueRect, float boxHeight)
		{
			return new Rect(keyValueRect.x + (keyValueRect.width * KEY_WIDTH_MULTIPLIER) - LINE_WIDTH / 2f + BOX_BORDER, 
							keyValueRect.y + BOX_BORDER,
							LINE_WIDTH,
							boxHeight - BOX_BORDER * 2f);
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
							keyValueRect.height - BOX_BORDER * 2f);
		}
	}
}
