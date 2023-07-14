using System;

namespace Padoru.Core
{
	public interface IFSMProxy<TState, TTrigger> where TState : Enum where TTrigger : Enum
	{
		void Setup(IFSM<TState, TTrigger> fsm);
		
		void SetTrigger(TTrigger trigger);
	}
}
