using Padoru.Core.Utils;
using UnityEngine;

namespace Padoru.Core
{
	public class FpsStatDisplay : IStatDisplay
	{
		private readonly FpsCounter fpsCounter;

		private string currentFpsCount = "0";

		public FpsStatDisplay(ITickManager tickManager)
		{
			fpsCounter = new FpsCounter(tickManager);
			fpsCounter.OnFpsChanged += OnFpsChanged;
		}
		
		public string GetStatText()
		{
			return "-------------------------------------------FPS: " + currentFpsCount;
		}

		public void Dispose()
		{
			fpsCounter.OnFpsChanged -= OnFpsChanged;
			fpsCounter.Dispose();
		}

		private void OnFpsChanged(string fps)
		{
			currentFpsCount = fps;
		}
	}
}
