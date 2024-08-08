using System.Collections.Generic;
using UnityEngine;

namespace Padoru.Core
{
	// TODO: Make width dynamic as well as height
	// TODO: Make it so it does not go off screen
	// TODO: Make this screen look better
	// TODO: Make it so we can toggle this with a key
	// TODO: Make it so this always appears in development builds and editor only
	// TODO: Make it so we have configs to set the default state (enabled/disabled)
	// TODO: Make it so there is a padoru context with this and some other basic stuff.
	
	public class StatsPanel : IGUIItem
	{
		private const string STATS_WINDOW_NAME = "Stats";
		private const int WINDOW_X_PADDING = 20;

		private readonly List<IStatDisplay> statDisplays;
		private readonly IGUIManager guiManager;
		
		private Rect windowRect;
		private bool isDragging;
		private Vector2 dragStartPos;

		public StatsPanel(List<IStatDisplay> statDisplays, IGUIManager guiManager, Rect windowRect)
		{
			this.statDisplays = statDisplays;
			this.guiManager = guiManager;
			this.windowRect = windowRect;

			guiManager.Register(this);
		}

		public void Dispose()
		{
			guiManager.Unregister(this);
		}

		public void OnGUI()
		{
			windowRect = GUILayout.Window(0, windowRect, DrawWindow, STATS_WINDOW_NAME);
			HandleDragging();
		}

		private void DrawWindow(int windowID)
		{
			var maxLabelWidth = 0f;
			var style = GUI.skin.label;
			
			foreach (var statDisplay in statDisplays)
			{
				var statText = statDisplay.GetStatText();
				var labelSize = style.CalcSize(new GUIContent(statText));
				maxLabelWidth = Mathf.Max(maxLabelWidth, labelSize.x);
				GUILayout.Label(statText);
			}

			windowRect.width = maxLabelWidth + WINDOW_X_PADDING; // Adding some padding
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
