using Godot;
using System;

namespace StateMachine.States
{
    public class PlayerSneakState : PlayerMovementState
    {
        public PlayerSneakState(Player player, Node2D parent)
            : base(player, "Sneak", 50, 0.2f, 0.8f, parent)
        {
        }

        public override bool CanTransitionTo(IState targetState)
        {
            if (targetState == null) return false;
            return targetState is PlayerWalkState || targetState is PlayerSprintState || targetState is PlayerHungryState;
        }
    }
}
