using System.Collections.Generic;
using UnityEngine;

namespace Padoru.Core
{
	public class StatsPanel : IGUIItem
	{
		private const string STATS_WINDOW_NAME = "Stats";
		private const int WINDOW_PADDING = 20;
		
		private static readonly Vector2 StartingPos = new (10, 10);

		private readonly List<IStatDisplay> statDisplays;
		private readonly IGUIManager guiManager;
		private readonly KeyCode toggleKey;
		
		private Rect windowRect;
		private bool isDragging;
		private Vector2 dragStartPos;
		private bool isVisible;
		
		public StatsPanel(List<IStatDisplay> statDisplays, IGUIManager guiManager, bool isVisible, KeyCode toggleKey)
		{
			this.statDisplays = statDisplays;
			this.guiManager = guiManager;
			this.isVisible = isVisible;
			this.toggleKey = toggleKey;

			windowRect = new Rect(StartingPos, Vector2.zero);
			
			guiManager.Register(this);
		}

		public void Dispose()
		{
			guiManager.Unregister(this);
		}

		public void OnGUI()
		{
			if (Event.current.type == EventType.KeyDown && Event.current.keyCode == toggleKey)
			{
				isVisible = !isVisible;
			}

			if (!isVisible)
			{
				return;
			}
			
			var maxWidth = 0f;
			var totalHeight = 0f;
			var style = GUI.skin.label;

			foreach (var statDisplay in statDisplays)
			{
				var statText = statDisplay.GetStatText();
				var labelSize = style.CalcSize(new GUIContent(statText));
				maxWidth = Mathf.Max(maxWidth, labelSize.x);
				totalHeight += labelSize.y;
			}

			windowRect.width = maxWidth + WINDOW_PADDING;
			windowRect.height = totalHeight + WINDOW_PADDING;

			windowRect = GUILayout.Window(0, windowRect, DrawWindow, STATS_WINDOW_NAME);
			HandleDragging();
		}

		private void DrawWindow(int windowID)
		{
			foreach (var statDisplay in statDisplays)
			{
				GUILayout.Label(statDisplay.GetStatText());
			}
			
			GUI.DragWindow(); // This ensures that the window is draggable
		}
		
		private void HandleDragging()
		{
			if (Event.current.type == EventType.MouseDown && windowRect.Contains(Event.current.mousePosition))
			{
				isDragging = true;
				dragStartPos = Event.current.mousePosition;
			}

			if (isDragging)
			{
				var currentMousePos = Event.current.mousePosition;
				var dragDelta = currentMousePos - dragStartPos;
				
				windowRect.position += dragDelta;
				dragStartPos = currentMousePos;

				if (Event.current.type == EventType.MouseUp)
				{
					isDragging = false;
				}
			}
		}
	}
}
