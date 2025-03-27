using Godot;
using System;
using StateMachine.States;
using OutCastSurvival.Entities;

namespace StateMachine
{
    public partial class PlayerStateMachineNode : Node
    {
        private StateMachine _stateMachine;
        private Player _player;
        private bool _isSneaking = false;
        private bool _isSprinting = false;

        private PlayerSneakState _sneakState;
        private PlayerWalkState _walkState;
        private PlayerSprintState _sprintState;

        [Export]
        public bool IsActive
        {
            get => _stateMachine?.IsActive ?? false;
            set
            {
                if (_stateMachine != null)
                    _stateMachine.SetActive(value);
            }
        }

        public override void _Ready()
        {
            _player = GetParent<Player>();
            if (_player == null)
            {
                GD.PrintErr("PlayerStateMachineNode must be a child of a Player node!");
                return;
            }

            InitializeStateMachine();
            IsActive = true;
        }

        private void InitializeStateMachine()
        {
            _stateMachine = new StateMachine();

            // Add movement states
            _sneakState = new PlayerSneakState(_player);
            _walkState = new PlayerWalkState(_player);
            _sprintState = new PlayerSprintState(_player);

            _stateMachine.AddState(_sneakState);
            _stateMachine.AddState(_walkState);
            _stateMachine.AddState(_sprintState);

            // Set initial state
            _stateMachine.SetState<PlayerWalkState>();
            _player.CurrentState = _walkState;
        }

        public override void _Process(double delta)
        {
            _stateMachine?.Update((float)delta);
        }

        public override void _UnhandledInput(InputEvent @event)
        {
            if (!IsActive) return;

            // Handle sneak hold
            if (@event.IsActionPressed("sneak"))
            {
                _isSneaking = true;
                UpdateMovementState();
            }
            else if (@event.IsActionReleased("sneak"))
            {
                _isSneaking = false;
                UpdateMovementState();
            }

            // Handle sprint hold
            if (@event.IsActionPressed("sprint"))
            {
                _isSprinting = true;
                UpdateMovementState();
            }
            else if (@event.IsActionReleased("sprint"))
            {
                _isSprinting = false;
                UpdateMovementState();
            }

            _stateMachine?.HandleInput(@event);
        }

        private void UpdateMovementState()
        {
            if (_isSneaking)
            {
                _stateMachine.SetState<PlayerSneakState>();
                _player.CurrentState = _sneakState;
            }
            else if (_isSprinting)
            {
                _stateMachine.SetState<PlayerSprintState>();
                _player.CurrentState = _sprintState;
            }
            else
            {
                _stateMachine.SetState<PlayerWalkState>();
                _player.CurrentState = _walkState;
            }
        }
    }
}
