using Godot;
using System;

public partial class SearchState : State
{
    public override void Enter()
    {
        GD.Print("Entering SearchState");
    }

    public override void Process(float delta)
    {
    }

    public override void Exit()
    {
        GD.Print("Exiting SearchState");
    }
}
