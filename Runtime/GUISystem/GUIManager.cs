using System.Collections.Generic;
using UnityEngine;

namespace Padoru.Core
{
	public class GUIManager : MonoBehaviour, IGUIManager, IInitializable, IShutdowneable
	{
		private readonly List<IGUIItem> guiItems = new ();

		private void OnGUI()
		{
			foreach (var guiItem in guiItems)
			{
				guiItem.OnGUI();
			}
		}

		public void Init()
		{
			Locator.Register<IGUIManager>(this);
		}

		public void Shutdown()
		{
			Locator.Unregister<IGUIManager>();
		}

		public void Register(IGUIItem guiItem)
		{
			if (guiItems.Contains(guiItem))
			{
				return;
			}

			guiItems.Add(guiItem);
		}

		public void Unregister(IGUIItem guiItem)
		{
			if (!guiItems.Contains(guiItem))
			{
				return;
			}

			guiItems.Remove(guiItem);
		}
	}
}
