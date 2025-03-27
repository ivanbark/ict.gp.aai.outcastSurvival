using Godot;
using System;

namespace StateMachine
{
    public abstract class BaseState : IState
    {
        public string StateName { get; protected set; }
        public StateMachine ParentStateMachine { get; set; }
        public StateMachine SubStateMachine { get; set; }
        public bool IsActive { get; set; }
        public IState ParentState { get; set; }
        protected readonly Guard _guard;
        protected readonly Node2D _parent;

        protected BaseState(Guard guard, string stateName, Node2D parent)
        {
            StateName = stateName;
            IsActive = false;
            _guard = guard;
            _parent = parent;
        }

        public virtual void Enter()
        {
            if (IsActive) return;
            IsActive = true;
            GD.Print($"{_parent.Name}: Entering {StateName}");

            if (SubStateMachine != null)
            {
                SubStateMachine.SetActive(true);
            }
        }

        public virtual void Exit()
        {
            if (!IsActive) return;
            IsActive = false;
            GD.Print($"{_parent.Name}: Exiting {StateName}");

            if (SubStateMachine != null)
            {
                SubStateMachine.SetActive(false);
            }
        }

        public virtual void Update(float delta)
        {
            if (!IsActive) return;
        }

        public virtual void HandleInput(InputEvent @event)
        {
            if (!IsActive) return;
        }

        public virtual void OnTransition(IState from, IState to)
        {
            if (!CanTransitionTo(to))
            {
                GD.PrintErr($"{_parent.Name}: Invalid transition from {from?.StateName ?? "null"} to {to?.StateName ?? "null"} in {StateName}");
                return;
            }

            GD.Print($"{_parent.Name}: Transitioning from {from?.StateName ?? "null"} to {to?.StateName ?? "null"}");
        }

        public virtual void OnChildStateEnter(IState childState)
        {
            GD.Print($"{_parent.Name}: Child state {childState.StateName} entered in {StateName}");
        }

        public virtual void OnChildStateExit(IState childState)
        {
            GD.Print($"{_parent.Name}: Child state {childState.StateName} exited in {StateName}");
        }

        public virtual void OnChildStateTransition(IState from, IState to)
        {
            GD.Print($"{_parent.Name}: Child state transition from {from?.StateName ?? "null"} to {to?.StateName ?? "null"} in {StateName}");
        }

        public virtual bool CanTransitionTo(IState targetState)
        {
            return targetState != null;
        }

        protected void TransitionTo<T>() where T : IState
        {
            if (ParentStateMachine == null)
            {
                GD.PrintErr($"{_parent.Name}: Cannot transition to {typeof(T).Name}: ParentStateMachine is null");
                return;
            }

            var targetState = ParentStateMachine.GetState<T>();
            if (targetState == null)
            {
                GD.PrintErr($"{_parent.Name}: Cannot transition to {typeof(T).Name}: State not found");
                return;
            }

            if (CanTransitionTo(targetState))
            {
                ParentStateMachine.SetState<T>();
            }
        }

        protected void RevertToPreviousState()
        {
            ParentStateMachine?.RevertToPreviousState();
        }

        protected void TransitionToChild<T>() where T : IState
        {
            if (SubStateMachine == null)
            {
                GD.PrintErr($"{_parent.Name}: Cannot transition to child state {typeof(T).Name}: SubStateMachine is null");
                return;
            }

            var targetState = SubStateMachine.GetState<T>();
            if (targetState == null)
            {
                GD.PrintErr($"{_parent.Name}: Cannot transition to child state {typeof(T).Name}: State not found");
                return;
            }

            SubStateMachine.SetState<T>();
        }

        protected void RevertToPreviousChildState()
        {
            SubStateMachine?.RevertToPreviousState();
        }
    }
}
