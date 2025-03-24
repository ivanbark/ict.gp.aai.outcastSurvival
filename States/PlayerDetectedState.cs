using Godot;
using System;

public partial class PlayerDetectedState : State
{
    private StateMachine _subStateMachine;

    protected override void OnInitialize()
    {
        _subStateMachine = GetNode<StateMachine>("SubStateMachine");
    }

    public override void Enter()
    {
        GD.Print("Entering PlayerDetectedState");
        _subStateMachine.SetActive(true);
    }

    public override void Exit()
    {
        GD.Print("Exiting PlayerDetectedState");
        _subStateMachine.SetActive(false);
    }
}
