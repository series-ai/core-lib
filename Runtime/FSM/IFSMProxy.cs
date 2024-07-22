using System;

namespace Padoru.Core
{
	public interface IFSMProxy<TState, TTrigger>
	{
		void Setup(IFSM<TState, TTrigger> fsm);
		
		void SetTrigger(TTrigger trigger);
		
		void SetState(TState state);
	}
}
