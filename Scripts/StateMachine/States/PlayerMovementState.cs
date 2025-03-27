using Godot;
using System;

namespace StateMachine.States
{
    public abstract class PlayerMovementState : BaseState
    {
        protected readonly Player _player;
        protected int _movementSpeed;
        protected float _noiseLevel;

        protected PlayerMovementState(Player player, string stateName, int movementSpeed, float noiseLevel)
            : base(null, stateName)
        {
            _player = player;
            _movementSpeed = movementSpeed;
            _noiseLevel = noiseLevel;
        }

        public override void Enter()
        {
            base.Enter();
            _player.MaxSpeed = _movementSpeed;
        }

        public override void Update(float delta)
        {
            base.Update(delta);
            HandleMovement(delta);
        }

        protected virtual void HandleMovement(float delta)
        {
            Vector2 inputDirection = Vector2.Zero;

            if (Input.IsActionPressed("move_up"))
                inputDirection += Vector2.Up;
            if (Input.IsActionPressed("move_down"))
                inputDirection += Vector2.Down;
            if (Input.IsActionPressed("move_left"))
                inputDirection += Vector2.Left;
            if (Input.IsActionPressed("move_right"))
                inputDirection += Vector2.Right;

            if (inputDirection != Vector2.Zero)
            {
                inputDirection = inputDirection.Normalized();
                _player.Velocity = inputDirection * _movementSpeed;
            }
            else
            {
                _player.Velocity = Vector2.Zero;
            }
        }

        public float GetNoiseLevel()
        {
            return _noiseLevel;
        }
    }
}
