using System;
using UnityEngine;

namespace Padoru.Core.Utils
{
	public class Timer : ITickable
	{
		private ITickManager tickManager;
		private ModifiableValue<float, FloatCalculator> tickInterval;
		private Action<float> callback;
		private float lastTickTime;

		public Timer(ModifiableValue<float, FloatCalculator> tickInterval, Action<float> callback)
		{
			tickManager = Locator.Get<ITickManager>();

			this.tickInterval = tickInterval;
			this.callback = callback;
		}

		public Timer(float tickInterval, Action<float> callback)
		{
			tickManager = Locator.Get<ITickManager>();

			this.tickInterval = new ModifiableValue<float, FloatCalculator>(tickInterval);
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
			if(Time.time - lastTickTime >= tickInterval.Value)
			{
				lastTickTime = Time.time;
				callback?.Invoke(deltaTime);
			}
		}
	}
}
