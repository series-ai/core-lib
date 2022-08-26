using UnityEditor;
using UnityEngine;

namespace Padoru.Core.Editor
{
	public static class GUIStyles
	{
		public static GUIStyle FoldableTitle;

		public static GUIStyle ListButtonStyle;

		static GUIStyles()
		{
			FoldableTitle = EditorStyles.foldout;
			FoldableTitle.fontStyle = FontStyle.Bold;

			var activeTexture = GetTextureFromColor(new Color(0.4f, 0.4f, 0.4f), 64, 64);

			ListButtonStyle = new GUIStyle();
			ListButtonStyle.alignment = TextAnchor.MiddleCenter;
			ListButtonStyle.active.background = activeTexture;
		}

		private static Texture2D GetTextureFromColor(Color color, int sizeX, int sizeY)
		{
			Texture2D texture = new Texture2D(sizeX, sizeY);

			for (int y = 0; y < texture.height; y++)
			{
				for (int x = 0; x < texture.width; x++)
				{
					texture.SetPixel(x, y, color);
				}
			}
			texture.Apply();

			return texture;
		}
	}
}
