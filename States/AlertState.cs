using Godot;
using System;
using OutCastSurvival.Entities;

namespace OutCastSurvival.State 
{
public partial class AlertState : State
{
    private StateMachine _subStateMachine;

    public override void Ready()
    {
        _subStateMachine = GetNode<StateMachine>("StateMachine");
    }

    public override void Enter()
    {
        GD.Print("Entering AlertState");
        _subStateMachine._Ready();
    }

    public override void Process(float delta)
    {
        _subStateMachine._Process(delta);
    }

    public override void Exit()
    {
        GD.Print("Exiting AlertState");
    }
}
}