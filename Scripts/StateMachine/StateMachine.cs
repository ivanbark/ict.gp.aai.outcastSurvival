using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StateMachine
{
    public class StateMachine
    {
        private readonly Dictionary<Type, IState> _states;
        private IState _currentState;
        private readonly Stack<IState> _stateHistory;
        private bool _isActive;
        private StateMachine _parentStateMachine;
        private IState _parentState;

        public IState CurrentState => _currentState;
        public bool IsActive => _isActive;
        public StateMachine ParentStateMachine => _parentStateMachine;
        public IState ParentState => _parentState;

        public event Action<IState, IState> OnStateChanged;
        public event Action<IState, IState> OnChildStateChanged;

        public StateMachine(StateMachine parentStateMachine = null, IState parentState = null)
        {
            _states = new Dictionary<Type, IState>();
            _stateHistory = new Stack<IState>();
            _parentStateMachine = parentStateMachine;
            _parentState = parentState;
        }

        public void AddState(IState state)
        {
            if (state == null)
                throw new ArgumentNullException(nameof(state));

            var stateType = state.GetType();
            if (_states.ContainsKey(stateType))
                throw new InvalidOperationException($"State of type {stateType} already exists!");

            state.ParentStateMachine = this;
            _states[stateType] = state;
        }

        public void SetState<T>() where T : IState
        {
            if (!_states.TryGetValue(typeof(T), out var newState))
                throw new InvalidOperationException($"State of type {typeof(T)} not found!");

            if (_currentState != null)
            {
                _currentState.Exit();
                _currentState.OnTransition(_currentState, newState);
                OnStateChanged?.Invoke(_currentState, newState);
            }

            _stateHistory.Push(_currentState);
            _currentState = newState;
            _currentState.Enter();

            if (_parentState != null)
            {
                _parentState.OnChildStateEnter(newState);
            }
        }

        public void RevertToPreviousState()
        {
            if (_stateHistory.Count == 0) return;

            var previousState = _stateHistory.Pop();
            if (_currentState != null)
            {
                _currentState.Exit();
                _currentState.OnTransition(_currentState, previousState);
                OnStateChanged?.Invoke(_currentState, previousState);
            }

            _currentState = previousState;
            _currentState.Enter();

            if (_parentState != null)
            {
                _parentState.OnChildStateEnter(previousState);
            }
        }

        public void Update(float delta)
        {
            if (!_isActive || _currentState == null) return;

            // Update current state
            _currentState.Update(delta);

            // Update sub-state machine if it exists and is active
            if (_currentState.SubStateMachine != null && _currentState.SubStateMachine.IsActive)
            {
                _currentState.SubStateMachine.Update(delta);
            }
        }

        public void HandleInput(InputEvent @event)
        {
            if (!_isActive || _currentState == null) return;
            _currentState.HandleInput(@event);
        }

        public void SetActive(bool active)
        {
            _isActive = active;
            if (_currentState != null)
            {
                if (active)
                    _currentState.Enter();
                else
                    _currentState.Exit();
            }
        }

        public void ClearHistory()
        {
            _stateHistory.Clear();
        }

        public void SetParentStateMachine(StateMachine parentStateMachine, IState parentState)
        {
            _parentStateMachine = parentStateMachine;
            _parentState = parentState;
        }

        public T GetState<T>() where T : IState
        {
            if (_states.TryGetValue(typeof(T), out var state))
                return (T)state;
            return default;
        }
    }
}
