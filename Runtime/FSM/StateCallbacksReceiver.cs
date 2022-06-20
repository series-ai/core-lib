namespace Padoru.Core
{
	public abstract class StateCallbacksReceiver
	{
		public StateCallbacksReceiver(State state)
		{
			state.OnStateEnterEvent += OnStateEnter;
			state.OnStateUpdateEvent += OnStateUpdate;
			state.OnStateExitEvent += OnStateExit;
		}

		protected abstract void OnStateExit();
		protected abstract void OnStateUpdate();
		protected abstract void OnStateEnter();
	}
}
