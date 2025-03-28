using Godot;
using System;
using StateMachine.States;

namespace StateMachine
{
    public partial class PlayerStateMachineNode : Node
    {
        private StateMachine _stateMachine;
        private Player _player;
        private Node2D _parent;
        private bool _isSneaking = false;
        private bool _isSprinting = false;
        private bool _isHungry = false;

        private PlayerSneakState _sneakState;
        private PlayerWalkState _walkState;
        private PlayerSprintState _sprintState;
        private PlayerHungryState _hungryState;

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
            _parent = (Node2D)GetParent<Player>();
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
            _sneakState = new PlayerSneakState(_player, _parent);
            _walkState = new PlayerWalkState(_player, _parent);
            _sprintState = new PlayerSprintState(_player, _parent);
            _hungryState = new PlayerHungryState(_player, _parent, _sneakState.GetNoiseLevel(), _sneakState.GetMovementSpeed());

            _stateMachine.AddState(_sneakState);
            _stateMachine.AddState(_walkState);
            _stateMachine.AddState(_sprintState);
            _stateMachine.AddState(_hungryState);

            // Set initial state
            _stateMachine.SetState<PlayerWalkState>();
            _player.CurrentState = _walkState;
        }

        public override void _Process(double delta)
        {
            _stateMachine?.Update((float)delta);

            if (_player.CurrentHunger <= _player.MaxHunger * 0.2f)
            {
                _isHungry = true;
                UpdateMovementState();
            }
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
            if (_isHungry)
            {
                _stateMachine.SetState<PlayerHungryState>();
                _player.CurrentState = _hungryState;
                _hungryState.SetSneaking(_isSneaking);
            }
            else if (_isSneaking)
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

        public void SetHungry(bool hungry)
        {
            _isHungry = hungry;
            UpdateMovementState();
        }

        public StateMachine GetStateMachine()
        {
            return _stateMachine;
        }
    }
}
