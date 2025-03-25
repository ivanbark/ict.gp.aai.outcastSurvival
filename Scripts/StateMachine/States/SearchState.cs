using Godot;
using System;

namespace StateMachine.States
{
    public class SearchState : BaseState
    {
        public SearchState(Guard guard)
            : base(guard, "Search")
        {
        }

        public override bool CanTransitionTo(IState targetState)
        {
            if (targetState == null) return false;

            // Allow transitions to PlayerDetectedState or PatrolState
            return targetState is PlayerDetectedState || targetState is PatrolState;
        }
    }
}
