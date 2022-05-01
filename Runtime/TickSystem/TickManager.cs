using System.Collections.Generic;
using UnityEngine;

namespace Padoru.Core
{
	public class TickManager : MonoBehaviour, ITickManager, IInitializable, IShutdowneable
	{
		private List<ITickable> tickables = new List<ITickable>();

		private void Update()
		{
			foreach (var tickable in tickables)
			{
				tickable.Tick(Time.deltaTime);
			}
		}

		public void Init()
		{
			Locator.RegisterService<ITickManager>(this);
		}

		public void Shutdown()
		{
			Locator.UnregisterService<ITickManager>();
		}

		public void Register(ITickable tickable)
		{
			if (tickables.Contains(tickable))
			{
				return;
			}

			tickables.Add(tickable);
		}

		public void Unregister(ITickable tickable)
		{
			if (!tickables.Contains(tickable))
			{
				return;
			}

			tickables.Remove(tickable);
		}
	}
}
