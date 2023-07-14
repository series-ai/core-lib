using System;

namespace Padoru.Core
{
	public interface IFSM<TState, TTrigger> where TState : Enum where TTrigger : Enum
	{
		TState CurrentStateId { get; }
		
		TState PreviousStateId { get; }
		
		bool IsActive { get; }

		void Start();
		
		void Stop();
		
		void AddTransition(TState initialState, TState targetState, TTrigger trigger);
		
		void AddStateCallbackReceiver(TState stateId, StateCallbacksReceiver stateCallbacksReceiver);

		void SetTrigger(TTrigger trigger);

		State GetState(TState stateId);
	}
}
