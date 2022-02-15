using System;

namespace Padoru.Core
{
	public interface IFSM<TState, TTrigger> where TState : Enum where TTrigger : Enum
	{
		public void AddTransition(TState initialState, TState targetState, TTrigger trigger);

		public void SetTrigger(TTrigger trigger);

		public State GetState(TState stateId);
	}
}
