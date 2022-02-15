using System;

namespace Padoru.Core
{
	public class Transition<TState, TTrigger> where TState : Enum where TTrigger : Enum
	{
		public TState initialState;
		public TState targetState;
		public TTrigger trigger;
	}
}
