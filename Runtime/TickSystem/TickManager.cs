using System.Collections.Generic;
using UnityEngine;

namespace Padoru.Core
{
	public class TickManager : MonoBehaviour, ITickManager, IInitializable, IShutdowneable
	{
		private readonly List<ITickable> tickables = new ();

		private void Update()
		{
			foreach (var tickable in tickables)
			{
				tickable.Tick(Time.deltaTime);
			}
		}

		public void Init()
		{
			Locator.Register<ITickManager>(this);
		}

		public void Shutdown()
		{
			Locator.Unregister<ITickManager>();
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
