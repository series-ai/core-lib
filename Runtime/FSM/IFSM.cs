using System;

namespace Padoru.Core
{
	public interface IFSM<TState, TTrigger> where TState : Enum where TTrigger : Enum
	{
		TState CurrentStateId { get; }
		
		TState PreviousStateId { get; }
		
		bool IsActive { get; }
		
		IFSMProxy<TState, TTrigger> Proxy { get; }

		void Start();

		void Start(TState initialState);
		
		void Stop();
		
		void AddTransition(TState initialState, TState targetState, TTrigger trigger);
		
		void AddStateCallbackReceiver(TState stateId, StateCallbacksReceiver stateCallbacksReceiver);

		void SetTrigger(TTrigger trigger);

		void SetState(TState stateId);

		State GetState(TState stateId);
	}
}
