using Godot;
using System;

namespace StateMachine.States
{
    public class PlayerSprintState : PlayerMovementState
    {
        public PlayerSprintState(Player player)
            : base(player, "Sprint", 200, 1.0f)
        {
        }

        public override bool CanTransitionTo(IState targetState)
        {
            if (targetState == null) return false;
            return targetState is PlayerSneakState || targetState is PlayerWalkState;
        }
    }
}
