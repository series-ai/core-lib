namespace Padoru.Core
{
	public abstract class StateCallbacksReceiver
	{
		private State state;

		public void Setup(State state)
		{
			this.state = state;
			
			state.OnStateEnterEvent += OnStateEnter;
			state.OnStateUpdateEvent += OnStateUpdate;
			state.OnStateExitEvent += OnStateExit;
		}

		public void Shutdown()
		{
			state.OnStateEnterEvent -= OnStateEnter;
			state.OnStateUpdateEvent -= OnStateUpdate;
			state.OnStateExitEvent -= OnStateExit;
		}

		protected abstract void OnStateExit();
		protected abstract void OnStateUpdate();
		protected abstract void OnStateEnter();
	}
}
