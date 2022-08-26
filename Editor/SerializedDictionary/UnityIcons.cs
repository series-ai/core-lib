using UnityEditor;
using UnityEngine;

namespace Padoru.Core.Editor
{
	public static class UnityIcons
	{
		public static GUIContent GetPlusIcon()
		{
			return IconContent("Toolbar Plus", "Add entry");
		}

		public static GUIContent GetMinusIcon()
		{
			return IconContent("Toolbar Minus", "Remove entry");
		}

		private static GUIContent IconContent(string name, string tooltip)
		{
			var builtinIcon = EditorGUIUtility.IconContent(name);
			return new GUIContent(builtinIcon.image, tooltip);
		}
	}
}
