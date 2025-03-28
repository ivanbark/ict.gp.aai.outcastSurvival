using Godot;
using OutCastSurvival.Entities;
using System;

namespace StateMachine.States
{
    public class PlayerSprintState : PlayerMovementState
    {
        public PlayerSprintState(Player player, Node2D parent)
            : base(player, "Sprint", 200, 1.0f, 1.5f, parent)
        {
        }

        public override bool CanTransitionTo(IState targetState)
        {
            if (targetState == null) return false;
            return targetState is PlayerSneakState || targetState is PlayerWalkState || targetState is PlayerHungryState;
        }
    }
}
