using System;
using System.Collections.Generic;
using System.Text;
using Debug = Padoru.Diagnostics.Debug;

namespace Padoru.Core
{
	public class FSM<TState, TTrigger> : IFSM<TState, TTrigger> where TState : Enum where TTrigger : Enum
	{
		private Dictionary<TState, State> states;
		private List<Transition<TState, TTrigger>> transitions;

		private State currentState;
		private TState initialState;

		public FSM(TState initialState)
		{
			states = new Dictionary<TState, State>();
			transitions = new List<Transition<TState, TTrigger>>();

			this.initialState = initialState;

			CreateStates();
		}

		public void Start()
		{
			ChangeState(initialState);
		}

		public void AddTransition(TState initialState, TState targetState, TTrigger trigger)
		{
			Debug.Log($"Added transition from '{initialState}' to '{targetState}' upon '{trigger}'");
			var transition = new Transition<TState, TTrigger>()
			{
				initialState = initialState,
				targetState = targetState,
				trigger = trigger,
			};

			transitions.Add(transition);
		}

		public void SetTrigger(TTrigger trigger)
		{
			Debug.Log($"Trigger set {trigger}");
			foreach (var transition in transitions)
			{
				if (!ShouldTransition(transition, trigger)) continue;

				ChangeState(transition.targetState);
			}
		}

		public State GetState(TState stateId)
		{
			return states[stateId];
		}

		private bool ShouldTransition(Transition<TState, TTrigger> transition, TTrigger trigger)
		{
			return states[transition.initialState].Equals(currentState) &&
				   transition.trigger.Equals(trigger);
		}

		private void CreateStates()
		{
			var sb = new StringBuilder();
			sb.Append("Created FSM with states:");

			var stateIds = Enum.GetValues(typeof(TState));
			foreach (var stateId in stateIds)
			{
				var state = new State(stateId.ToString());
				states.Add((TState)stateId, state);
				sb.Append(Environment.NewLine);
				sb.Append($" {stateId.ToString()}");
			}

			Debug.Log(sb);
		}

		private void ChangeState(TState stateId)
		{
			if(currentState != null)
			{
				currentState.OnStateExit();
			}

			currentState = states[stateId];
			currentState.OnStateEnter();
		}
	}
}
