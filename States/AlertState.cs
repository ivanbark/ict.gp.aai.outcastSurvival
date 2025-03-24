using Godot;
using System;

public partial class AlertState : State
{
    private StateMachine _subStateMachine;

    protected override void OnInitialize()
    {
        _subStateMachine = GetNode<StateMachine>("SubStateMachine");
    }

    public override void Enter()
    {
        GD.Print("Entering AlertState");
        _subStateMachine.SetActive(true);
    }

    public override void Exit()
    {
        GD.Print("Exiting AlertState");
        _subStateMachine.SetActive(false);
    }
}
