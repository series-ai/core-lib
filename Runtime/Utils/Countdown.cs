using System;

using Debug = Padoru.Diagnostics.Debug;

namespace Padoru.Core.Utils
{
	public class Countdown : ITickable
	{
		private ITickManager tickManager;
		private float startTime;
		public float currentTime;

		public event Action<float> OnTimeChanged;
		public event Action OnCountdownEnded;

		public Countdown(float startTime)
		{
			this.startTime = startTime;

			tickManager = Locator.Get<ITickManager>();
		}

		~Countdown()
		{
			Stop();
		}

		public void Start()
		{
			Debug.Log($"Countdown started. Time: {startTime}");

			currentTime = startTime;
			tickManager.Register(this);
		}

		public void Stop()
		{
			Debug.Log("Countdown stopped");

			tickManager.Unregister(this);
		}

		public void Tick(float deltaTime)
		{
			if(currentTime > 0)
			{
				currentTime -= deltaTime;
				OnTimeChanged?.Invoke(currentTime);

				if(currentTime <= 0)
				{
					OnCountdownEnded?.Invoke();
					Stop();
				}
			}
		}
	}
}
