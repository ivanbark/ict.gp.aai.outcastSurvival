 using Godot;
using System;
using System.Collections.Generic;

using OutCastSurvival.State;
using OutCastSurvival.Entities;

namespace OutCastSurvival 
{
public partial class StateMachine : Node
{
	[Export] public NodePath initialState;
	private Dictionary<string, State.State> _states;
	private State.State _currentState;
	
	public Guard Guard;
	
	public override void _Ready()
	{
		Guard = GetParent<Guard>();
		
		_states = new Dictionary<string, State.State>();
		foreach (Node node in GetChildren())
		{
			if (node is State.State s)
			{
				_states.Add(node.Name, s);
				s.fsm = this;
				s.Ready();
			}
		}
		
		_currentState = GetNode<State.State>(initialState);
		_currentState.Enter();
	}

	public override void _Process(double delta)
	{
		_currentState.Process((float) delta);
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		_currentState.HandleInput(@event);
	}

	public void TransitionTo(string key)
	{
		if (!_states.ContainsKey(key) || _currentState == _states[key])
			return;
		
		_currentState.Exit();
		_currentState = _states[key];
		_currentState.Enter();
	}
}
}
