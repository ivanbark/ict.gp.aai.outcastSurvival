using Godot;
using System;

public partial class State : Node
{
    public StateMachine fsm;
    protected bool _isInitialized = false;

    public virtual void Initialize()
    {
        if (_isInitialized) return;
        _isInitialized = true;
        OnInitialize();
    }

    protected virtual void OnInitialize() {}
    public virtual void Enter() {}
    public virtual void Exit() {}
    public virtual void Process(float delta) {}
    public virtual void HandleInput(InputEvent @event) {}

    protected void TransitionTo(string key)
    {
        fsm?.TransitionTo(key);
    }

    protected void TransitionToParent(string key)
    {
        fsm?.TransitionToParent(key);
    }
}
