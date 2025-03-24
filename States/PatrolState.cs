using Godot;
using System;

public partial class PatrolState : State
{
    private StateMachine _subStateMachine;

    protected override void OnInitialize()
    {
        _subStateMachine = GetNode<StateMachine>("SubStateMachine");
    }

    public override void Enter()
    {
        GD.Print("Entering PatrolState");
        _subStateMachine.SetActive(true);
    }

    public override void Process(float delta)
    {
        // No need to manually call _Process on subStateMachine
        // It will be called automatically by Godot when active
    }

    public override void Exit()
    {
        GD.Print("Exiting PatrolState");
        _subStateMachine.SetActive(false);
    }
}
