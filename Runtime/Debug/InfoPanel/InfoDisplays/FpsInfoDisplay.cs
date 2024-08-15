using Padoru.Core.Utils;
using UnityEngine;

namespace Padoru.Core
{
	public class FpsInfoDisplay : IInfoDisplay
	{
		private readonly FpsCounter fpsCounter;

		private string currentFpsCount = "0";

		public FpsInfoDisplay(ITickManager tickManager)
		{
			fpsCounter = new FpsCounter(tickManager);
			fpsCounter.OnFpsChanged += OnFpsChanged;
		}
		
		public string GetInfoText()
		{
			return "FPS: " + currentFpsCount;
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
