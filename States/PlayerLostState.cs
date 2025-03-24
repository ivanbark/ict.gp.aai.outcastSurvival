using Godot;
using System;

public partial class PlayerLostState : State
{
    private StateMachine _subStateMachine;

    protected override void OnInitialize()
    {
        _subStateMachine = GetNode<StateMachine>("SubStateMachine");
    }

    public override void Enter()
    {
        GD.Print("Entering PlayerLostState");
        _subStateMachine.SetActive(true);
    }

    public override void Exit()
    {
        GD.Print("Exiting PlayerLostState");
        _subStateMachine.SetActive(false);
    }
}
