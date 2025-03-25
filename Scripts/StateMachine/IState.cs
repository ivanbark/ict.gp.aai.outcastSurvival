using Godot;
using System;

namespace StateMachine
{
    public interface IState
    {
        string StateName { get; }
        StateMachine ParentStateMachine { get; set; }
        StateMachine SubStateMachine { get; set; }
        bool IsActive { get; set; }

        void Enter();
        void Exit();
        void Update(float delta);
        void HandleInput(InputEvent @event);
        void OnTransition(IState from, IState to);

        // Hierarchical state machine methods
        void OnChildStateEnter(IState childState);
        void OnChildStateExit(IState childState);
        void OnChildStateTransition(IState from, IState to);

        // State validation
        bool CanTransitionTo(IState targetState);
    }
}
