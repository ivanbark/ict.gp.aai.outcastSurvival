using Godot;
using System;

public partial class PatrolPathState : State
{
    public override void Enter()
    {
        GD.Print("Entering PatrolPathState");
    }

    public override void Process(float delta)
    {
    }

    public override void Exit()
    {
        GD.Print("Exiting PatrolPathState");
    }
}
