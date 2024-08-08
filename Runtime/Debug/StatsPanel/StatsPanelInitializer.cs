using System.Collections.Generic;
using UnityEngine;

namespace Padoru.Core
{
	public class StatsPanelInitializer : MonoBehaviour, IInitializable, IShutdowneable
	{
		[SerializeField] private KeyCode toggleKey = KeyCode.S;
		[SerializeField] private bool startsEnabled;
		
		private StatsPanel statsPanel;
		
		public void Init()
		{
			var guiManager = Locator.Get<IGUIManager>();
			
			var statDisplays = new List<IStatDisplay>()
			{
				new FpsStatDisplay(Locator.Get<ITickManager>()),
			};
			
			statsPanel = new StatsPanel(statDisplays, guiManager, startsEnabled, toggleKey);
		}

		public void Shutdown()
		{
			statsPanel.Dispose();
		}
	}
}
