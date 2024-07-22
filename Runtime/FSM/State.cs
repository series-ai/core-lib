using System;
using Padoru.Diagnostics;

namespace Padoru.Core
{
	public class State
	{
		private string stateName;
		private readonly string debugChannel;

		public event Action OnStateEnterEvent;
		public event Action OnStateUpdateEvent;
		public event Action OnStateExitEvent;

		public State(string stateName, string debugChannel)
		{
			this.stateName = stateName;
			this.debugChannel = debugChannel;
		}

		internal void OnStateEnter()
		{
			Debug.Log(stateName, debugChannel);
			OnStateEnterEvent?.Invoke();
		}

		internal void OnStateUpdate()
		{
			OnStateUpdateEvent?.Invoke();
		}

		internal void OnStateExit()
		{
			Debug.Log(stateName, debugChannel);
			OnStateExitEvent?.Invoke();
		}
	}
}
