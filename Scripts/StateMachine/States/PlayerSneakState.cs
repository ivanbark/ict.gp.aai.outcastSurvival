using Godot;
using System;

namespace StateMachine.States
{
    public class PlayerSneakState : PlayerMovementState
    {
        public PlayerSneakState(Player player)
            : base(player, "Sneak", 50, 0.2f)
        {
        }

        public override bool CanTransitionTo(IState targetState)
        {
            if (targetState == null) return false;
            return targetState is PlayerWalkState || targetState is PlayerSprintState;
        }
    }
}
