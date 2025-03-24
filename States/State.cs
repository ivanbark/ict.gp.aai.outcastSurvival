using Godot;
using System;
using OutCastSurvival;

namespace OutCastSurvival.State 
{
public partial class State : Node
{
    public StateMachine fsm;
    public virtual void Ready() {}
    public virtual void Enter() {}
    public virtual void Exit() {}
    public virtual void Process(float delta) {}
    public virtual void HandleInput(InputEvent @event) {}
}
}