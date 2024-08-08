using Padoru.Core.Utils;
using UnityEngine;

namespace Padoru.Core
{
	public class StatsPanel : IGUIItem
	{
		private const string STATS_WINDOW_NAME = "Stats";
		
		private readonly IGUIManager guiManager;
		private readonly FpsCounter fpsCounter;
		
		private Rect windowRect;
		private bool isDragging;
		private Vector2 dragStartPos;
		private string currentFps = "0";

		public StatsPanel(ITickManager tickManager, IGUIManager guiManager, Rect windowRect)
		{
			this.guiManager = guiManager;
			this.windowRect = windowRect;
			
			fpsCounter = new FpsCounter(tickManager);
			fpsCounter.OnFpsChanged += OnFpsChanged;

			guiManager.Register(this);
		}

		public void OnGUI()
		{
			windowRect = GUILayout.Window(0, windowRect, DrawWindow, STATS_WINDOW_NAME);
		}

		public void Shutdown()
		{
			fpsCounter.OnFpsChanged -= OnFpsChanged;
			fpsCounter.Shutdown();

			guiManager.Unregister(this);
		}

		private void OnFpsChanged(string fps)
		{
			currentFps = fps;
		}

		private void DrawWindow(int windowID)
		{
			GUILayout.Label("FPS: " + currentFps);

			if (Event.current.type == EventType.MouseDown && windowRect.Contains(Event.current.mousePosition))
			{
				isDragging = true;
				dragStartPos = Event.current.mousePosition;
			}

			if (isDragging)
			{
				Vector2 currentMousePos = Event.current.mousePosition;
				Vector2 dragDelta = currentMousePos - dragStartPos;
				windowRect.position += dragDelta;
				dragStartPos = currentMousePos;

				if (Event.current.type == EventType.MouseUp)
				{
					isDragging = false;
				}
			}

			GUI.DragWindow();
		}
	}
}
