using System;
using UnityEngine;

namespace Padoru.Core.Utils
{
	public class Timer : ITickable
	{
		private ITickManager tickManager;
		private float tickInterval;
		private Action callback;
		private float lastTickTime;

		public Timer(float tickInterval, Action callback)
		{
			tickManager = Locator.GetService<ITickManager>();

			this.tickInterval = tickInterval;
			this.callback = callback;
		}

		~Timer()
		{
			Stop();
		}

		public void Start()
		{
			lastTickTime = Time.time;
			tickManager.Register(this);
		}

		public void Stop()
		{
			tickManager.Unregister(this);
		}

		public void Tick(float deltaTime)
		{
			if(Time.time - lastTickTime >= tickInterval)
			{
				lastTickTime = Time.time;
				callback?.Invoke();
			}
		}
	}
}
