using Godot;
using System;

namespace StateMachine.States
{
    public class PlayerHungryState : PlayerMovementState
    {
        public PlayerHungryState(Player player, Node2D parent)
            : base(player, "Hungry", 60, 0.7f, 1.2f, parent)
        {
        }

        public override bool CanTransitionTo(IState targetState)
        {
            if (targetState == null) return false;
            return targetState is PlayerSneakState || targetState is PlayerSprintState || targetState is PlayerWalkState;
        }
    }
}
