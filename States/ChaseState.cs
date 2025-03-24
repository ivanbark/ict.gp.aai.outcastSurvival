using Godot;
using System;

public partial class ChaseState : State
{
    private Guard _guard;

    protected override void OnInitialize()
    {
        _guard = fsm.Guard;
    }

    public override void Enter()
    {
        GD.Print("Entering ChaseState");
    }

    public override void Exit()
    {
        GD.Print("Exiting ChaseState");
        _guard.Velocity = Vector2.Zero;
    }

    public override void Process(float delta)
    {
        GD.Print("Processing ChaseState");
        if (_guard == null) return;

        _guard.ChasePlayer(delta);

        if (_guard.Position.DistanceTo(_guard.Player.Position) <= _guard.AttackRange - 5)
        {
            TransitionTo("Attack");
        }
    }
}
