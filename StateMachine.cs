using Godot;
using System;
using System.Collections.Generic;

public partial class StateMachine : Node
{
	[Export] public NodePath initialState;
	private Dictionary<string, State> _states;
	private State _currentState;
	private StateMachine _parentStateMachine;
	private State _parentState;
	private bool _isActive = false;
	private Guard _guard;

	public Guard Guard
	{
		get
		{
			if (_guard == null)
			{
				// First try to get from parent state machine
				if (_parentStateMachine != null)
				{
					_guard = _parentStateMachine.Guard;
				}
				// If still null, try to find Guard by traversing up the tree
				if (_guard == null)
				{
					_guard = FindGuardInParentTree();
				}
			}
			return _guard;
		}
	}

	private Guard FindGuardInParentTree()
	{
		var current = GetParent();
		while (current != null)
		{
			if (current is Guard guard)
			{
				return guard;
			}
			current = current.GetParent();
		}
		return null;
	}

	public override void _Ready()
	{
		_states = new Dictionary<string, State>();

		// Find parent state machine if this is a nested state machine
		var parent = GetParent();
		if (parent is State parentState)
		{
			_parentState = parentState;
			_parentStateMachine = parentState.fsm;
		}
		else if (parent is StateMachine parentSM)
		{
			_parentStateMachine = parentSM;
		}

		// Initialize all child states
		foreach (Node node in GetChildren())
		{
			if (node is State s)
			{
				_states.Add(node.Name, s);
				s.fsm = this;
				s.Initialize();
			}
		}

		// Set initial state
		if (initialState != null)
		{
			_currentState = GetNode<State>(initialState);
			_currentState.Enter();
		}

		// Disable processing by default
		SetProcess(false);
		SetProcessUnhandledInput(false);
	}

	public override void _Process(double delta)
	{
		if (!_isActive) return;
		_currentState?.Process((float)delta);
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if (!_isActive) return;
		_currentState?.HandleInput(@event);
	}

	public void TransitionTo(string key)
	{
		if (!_states.ContainsKey(key) || _currentState == _states[key])
			return;

		_currentState?.Exit();
		_currentState = _states[key];
		_currentState.Enter();
	}

	public void TransitionToParent(string key)
	{
		if (_parentStateMachine != null)
		{
			_parentStateMachine.TransitionTo(key);
		}
	}

	public State GetCurrentState()
	{
		return _currentState;
	}

	public StateMachine GetParentStateMachine()
	{
		return _parentStateMachine;
	}

	public void SetActive(bool active)
	{
		_isActive = active;
		SetProcess(active);
		SetProcessUnhandledInput(active);
	}

	public bool IsActive()
	{
		return _isActive;
	}
}
