using Godot;
using System;

namespace StateMachine.States
{
    public class IdleState : BaseState
    {
        public IdleState(Guard guard)
            : base(guard, "Idle")
        {
        }

        public override bool CanTransitionTo(IState targetState)
        {
            if (targetState == null) return false;

            // Allow transitions to PatrolPathState or AlertState
            return targetState is PatrolPathState || targetState is AlertState;
        }
    }
}
