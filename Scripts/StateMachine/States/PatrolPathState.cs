using Godot;
using OutCastSurvival.Entities;
using System;

namespace StateMachine.States
{
    public class PatrolPathState : BaseState
    {
        public PatrolPathState(Guard guard, Node2D parent)
            : base(guard, "PatrolPath", parent)
        {
        }

        public override bool CanTransitionTo(IState targetState)
        {
            if (targetState == null) return false;

            // Allow transitions to IdleState or AlertState
            return targetState is IdleState || targetState is AlertState;
        }
    }
}
