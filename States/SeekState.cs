using Godot;
using System;

public partial class SeekState : State
{
    public override void Enter()
    {
        GD.Print("Entering SeekState");
    }

    public override void Process(float delta)
    {
    }

    public override void Exit()
    {
        GD.Print("Exiting SeekState");
    }
}
