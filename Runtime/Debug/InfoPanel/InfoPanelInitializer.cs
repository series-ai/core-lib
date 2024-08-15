using System.Collections.Generic;
using UnityEngine;

namespace Padoru.Core
{
	public class InfoPanelInitializer : MonoBehaviour, IInitializable, IShutdowneable
	{
		[SerializeField] private KeyCode toggleKey = KeyCode.S;
		[SerializeField] private bool startsEnabled;
		
		private InfoPanel infoPanel;
		
		public void Init()
		{
			var guiManager = Locator.Get<IGUIManager>();
			
			var statDisplays = new List<IInfoDisplay>()
			{
				new FpsInfoDisplay(Locator.Get<ITickManager>()),
			};
			
			infoPanel = new InfoPanel(statDisplays, guiManager, startsEnabled, toggleKey);
		}

		public void Shutdown()
		{
			infoPanel.Dispose();
		}
	}
}
