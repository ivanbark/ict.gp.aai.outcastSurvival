using Godot;
using OutCastSurvival.Entities;
using System;

namespace StateMachine.States
{
    public class PlayerWalkState : PlayerMovementState
    {
        public PlayerWalkState(Player player)
            : base(player, "Walk", 140, 0.5f)
        {
        }

        public override bool CanTransitionTo(IState targetState)
        {
            if (targetState == null) return false;
            return targetState is PlayerSneakState || targetState is PlayerSprintState;
        }
    }
}
