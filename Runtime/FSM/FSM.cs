using System;
using System.Collections.Generic;
using System.Text;
using Debug = Padoru.Diagnostics.Debug;

namespace Padoru.Core
{
	public class FSM<TState, TTrigger> : ITickable, IFSM<TState, TTrigger> where TState : Enum where TTrigger : Enum
	{
		private readonly Dictionary<TState, State> states;
		private readonly List<Transition<TState, TTrigger>> transitions;
		private readonly TState initialStateId;
		private readonly ITickManager tickManager;

		private State currentState;

		public TState CurrentStateId { get; private set; }
		public TState PreviousStateId { get; private set; }
		public bool IsActive { get; private set; }

		public FSM(TState initialStateId)
		{
			states = new Dictionary<TState, State>();
			transitions = new List<Transition<TState, TTrigger>>();

			this.initialStateId = initialStateId;

			tickManager = Locator.Get<ITickManager>();

			CreateStates();
		}

		public void Start()
		{
			if (IsActive)
			{
				throw new Exception("Could not start FSM, it is already active");
			}

			PreviousStateId = initialStateId;
			ChangeState(initialStateId);
			IsActive = true;
			tickManager.Register(this);
		}

		public void Stop()
		{
			if (!IsActive)
			{
				throw new Exception("Could not stop FSM, it is not active");
			}

			currentState?.OnStateExit();
			currentState = null;
			IsActive = false;
			tickManager.Unregister(this);
		}

		public void Tick(float deltaTime)
		{
			currentState?.OnStateUpdate();
		}

		public void AddTransition(TState initialState, TState targetState, TTrigger trigger)
		{
			var transition = new Transition<TState, TTrigger>()
			{
				initialState = initialState,
				targetState = targetState,
				trigger = trigger,
			};

			if (IsTransitionAlreadyRegistered(transition))
			{
				throw new Exception("Could not add transition, it was already registered");
			}

			Debug.Log($"Added transition from '{initialState}' to '{targetState}' upon '{trigger}'");

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
			return transition.initialState.Equals(CurrentStateId) && transition.trigger.Equals(trigger);
		}

		private bool IsTransitionAlreadyRegistered(Transition<TState, TTrigger> transition)
		{
			foreach (var existingTransition in transitions)
			{
				if (existingTransition.Equals(transition))
				{
					return true;
				}
			}

			return false;
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
				sb.Append($" {stateId}");
			}

			Debug.Log(sb);
		}

		private void ChangeState(TState stateId)
		{
			if (currentState != null)
			{
				PreviousStateId = CurrentStateId;
				currentState.OnStateExit();
			}

			currentState = GetState(stateId);
			CurrentStateId = stateId;

			if (currentState != null)
			{
				currentState.OnStateEnter();
			}
		}
	}
}
