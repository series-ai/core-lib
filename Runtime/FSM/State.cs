using System;
using Padoru.Diagnostics;

namespace Padoru.Core
{
	public class State
	{
		private string stateName;

		public event Action OnStateEnterEvent;
		public event Action OnStateExitEvent;

		public State(string stateName)
		{
			this.stateName = stateName;
		}

		internal void OnStateEnter()
		{
			Debug.Log(stateName);
			OnStateEnterEvent?.Invoke();
		}

		internal void OnStateExit()
		{
			Debug.Log(stateName);
			OnStateExitEvent?.Invoke();
		}
	}
}
