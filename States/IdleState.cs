using Godot;
using System;

public partial class IdleState : State
{
    public override void Enter()
    {
        GD.Print("Entering IdleState");
    }

    public override void Process(float delta)
    {
    }

    public override void Exit()
    {
        GD.Print("Exiting IdleState");
    }
}
