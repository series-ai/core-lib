using UnityEditor;
using UnityEngine;

namespace Padoru.Core.Editor
{
	public static class GUIStyles
	{
		public static GUIStyle FoldableTitle;

		static GUIStyles()
		{
			FoldableTitle = EditorStyles.foldout;
			FoldableTitle.fontStyle = FontStyle.Bold;
		}
	}
}
