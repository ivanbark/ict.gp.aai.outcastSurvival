using Godot;
using System;

namespace StateMachine.States
{
    public class PlayerHungryState : PlayerMovementState
    {
        private const int _hungerMovementSpeed = 60;
        private int _sneakMovementSpeed;
        private const float _hungerNoiseLevel = 0.7f;
        private float _sneakNoiseLevel;

        public PlayerHungryState(Player player, Node2D parent, float sneakNoiseLevel = 0.2f, int sneakMovementSpeed = 30)
            : base(player, "Hungry", _hungerMovementSpeed, 0.7f, 1.2f, parent)
        {
            _sneakNoiseLevel = sneakNoiseLevel;
            _sneakMovementSpeed = sneakMovementSpeed;
        }

        public void SetSneaking(bool sneaking)
        {
            _movementSpeed = sneaking ? _sneakMovementSpeed : _hungerMovementSpeed;
            _noiseLevel = sneaking ? _sneakNoiseLevel : _hungerNoiseLevel;
        }

        public override bool CanTransitionTo(IState targetState)
        {
            if (targetState == null) return false;
            return targetState is PlayerSneakState || targetState is PlayerSprintState || targetState is PlayerWalkState;
        }
    }
}
