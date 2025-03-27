using Godot;
using System;
using StateMachine.States;

namespace StateMachine
{
    public partial class GuardStateMachineNode : Node
    {
        private StateMachine _rootStateMachine;
        private Guard _guard;
        private Node2D _parent;

        [Export]
        public bool IsActive
        {
            get => _rootStateMachine?.IsActive ?? false;
            set
            {
                if (_rootStateMachine != null)
                    _rootStateMachine.SetActive(value);
            }
        }

        public override void _Ready()
        {
            _guard = GetParent<Guard>();
            _parent = (Node2D)GetParent<Guard>();
            if (_guard == null)
            {
                GD.PrintErr("GuardStateMachineNode must be a child of a Guard node!");
                return;
            }

            InitializeStateMachine();
            IsActive = true;  // Set the state machine to active after initialization
        }

        private void InitializeStateMachine()
        {
            _rootStateMachine = new StateMachine();
            InitializePatrolStateMachine();
            InitializeAlertStateMachine();
            SetInitialState();
            SubscribeToEvents();
        }

        private void InitializePatrolStateMachine()
        {
            var patrolStateMachine = new StateMachine(_rootStateMachine, null);
            var patrolState = new PatrolState(_guard, _parent);
            patrolState.SubStateMachine = patrolStateMachine;
            _rootStateMachine.AddState(patrolState);

            // Add Patrol sub-states
            var idleState = new IdleState(_guard, _parent);
            var patrolPathState = new PatrolPathState(_guard, _parent);
            patrolStateMachine.AddState(idleState);
            patrolStateMachine.AddState(patrolPathState);
        }

        private void InitializeAlertStateMachine()
        {
            var alertStateMachine = new StateMachine(_rootStateMachine, null);
            var alertState = new AlertState(_guard, _parent);
            alertState.SubStateMachine = alertStateMachine;
            _rootStateMachine.AddState(alertState);

            InitializePlayerLostStateMachine(alertStateMachine, alertState);
            InitializePlayerDetectedStateMachine(alertStateMachine, alertState);
        }

        private void InitializePlayerLostStateMachine(StateMachine alertStateMachine, AlertState alertState)
        {
            var playerLostStateMachine = new StateMachine(alertStateMachine, alertState);
            var playerLostState = new PlayerLostState(_guard, _parent);
            playerLostState.SubStateMachine = playerLostStateMachine;
            alertStateMachine.AddState(playerLostState);

            // Add PlayerLost sub-states
            var seekState = new SeekState(_guard, _parent);
            var searchState = new SearchState(_guard, _parent);
            playerLostStateMachine.AddState(seekState);
            playerLostStateMachine.AddState(searchState);
        }

        private void InitializePlayerDetectedStateMachine(StateMachine alertStateMachine, AlertState alertState)
        {
            var playerDetectedStateMachine = new StateMachine(alertStateMachine, alertState);
            var playerDetectedState = new PlayerDetectedState(_guard, _parent);
            playerDetectedState.SubStateMachine = playerDetectedStateMachine;
            alertStateMachine.AddState(playerDetectedState);

            // Add PlayerDetected sub-states
            var chaseState = new ChaseState(_guard, _parent);
            var attackState = new AttackState(_guard, _parent);
            playerDetectedStateMachine.AddState(chaseState);
            playerDetectedStateMachine.AddState(attackState);
        }

        private void SetInitialState()
        {
            _rootStateMachine.SetState<PatrolState>();
            var patrolState = _rootStateMachine.GetState<PatrolState>();
            if (patrolState?.SubStateMachine != null)
            {
                patrolState.SubStateMachine.SetState<IdleState>();
            }
        }

        private void SubscribeToEvents()
        {
            _rootStateMachine.OnStateChanged += OnStateChanged;
            _rootStateMachine.OnChildStateChanged += OnChildStateChanged;
        }

        public override void _Process(double delta)
        {
            _rootStateMachine?.Update((float)delta);
        }

        public override void _UnhandledInput(InputEvent @event)
        {
            _rootStateMachine?.HandleInput(@event);
        }

        public void TransitionToAlert()
        {
            if (_rootStateMachine == null) return;

            // Check if we're already in a player detected state
            var currentState = _rootStateMachine.CurrentState;
            if (currentState is AlertState alertState)
            {
                var alertSubState = alertState.SubStateMachine?.CurrentState;
                if (alertSubState is PlayerDetectedState)
                {
                    return; // Already in player detected state, no need to transition
                }
            }

            _rootStateMachine.SetState<AlertState>();
            var newAlertState = _rootStateMachine.GetState<AlertState>();
            if (newAlertState?.SubStateMachine != null)
            {
                newAlertState.SubStateMachine.SetState<PlayerDetectedState>();
                var playerDetectedState = newAlertState.SubStateMachine.GetState<PlayerDetectedState>();
                if (playerDetectedState?.SubStateMachine != null)
                {
                    playerDetectedState.SubStateMachine.SetState<ChaseState>();
                }
            }
        }

        public void TransitionToPatrol()
        {
            if (_rootStateMachine == null || _rootStateMachine.CurrentState == _rootStateMachine.GetState<PatrolState>()) return;

            _rootStateMachine.SetState<PatrolState>();
            var patrolState = _rootStateMachine.GetState<PatrolState>();
            if (patrolState?.SubStateMachine != null)
            {
                patrolState.SubStateMachine.SetState<IdleState>();
            }
        }

        public IState GetCurrentState()
        {
            var rootState = _rootStateMachine.CurrentState;
            if (rootState.SubStateMachine != null)
            {
                var subState = rootState.SubStateMachine.CurrentState;
                if (subState.SubStateMachine != null)
                {
                    subState = subState.SubStateMachine.CurrentState;
                }
                return subState;
            }
            return rootState;
        }

        private void OnStateChanged(IState from, IState to)
        {
            GD.Print($"Guard: State changed from {from?.StateName ?? "null"} to {to?.StateName ?? "null"}");
        }

        private void OnChildStateChanged(IState from, IState to)
        {
            GD.Print($"Guard: Child state changed from {from?.StateName ?? "null"} to {to?.StateName ?? "null"}");
        }

        public override void _ExitTree()
        {
            if (_rootStateMachine != null)
            {
                _rootStateMachine.OnStateChanged -= OnStateChanged;
                _rootStateMachine.OnChildStateChanged -= OnChildStateChanged;
                _rootStateMachine.SetActive(false);
            }
        }
    }
}
