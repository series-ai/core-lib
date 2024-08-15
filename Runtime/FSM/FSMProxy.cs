using System;

namespace Padoru.Core
{
	public class FSMProxy<TState, TTrigger> : IFSMProxy<TState, TTrigger>
	{
		private IFSM<TState, TTrigger> fsm;
		
		public FSMProxy(IFSM<TState, TTrigger> fsm)
		{
			Setup(fsm);
		}
		
		public void Setup(IFSM<TState, TTrigger> fsm)
		{
			this.fsm = fsm;
		}

		public void SetTrigger(TTrigger trigger)
		{
			fsm.SetTrigger(trigger);
		}

		public void SetState(TState state)
		{
			fsm.SetState(state);
		}
	}
}
