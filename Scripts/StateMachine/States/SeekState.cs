using Godot;
using System;

namespace StateMachine.States
{
    public class SeekState : BaseState
    {
        public SeekState(Guard guard)
            : base(guard, "Seek")
        {
        }

        public override bool CanTransitionTo(IState targetState)
        {
            if (targetState == null) return false;

            // Allow transitions to SearchState
            return targetState is SearchState;
        }
    }
}
