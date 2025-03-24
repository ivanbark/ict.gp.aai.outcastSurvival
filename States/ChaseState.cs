using Godot;
using System;

public partial class ChaseState : State
{
    private Guard _guard;

    public override void Ready()
    {
        _guard = fsm.Guard;
    }
    public override void Enter()
    {
    }

    public override void Exit()
    {
        _guard.Velocity = Vector2.Zero;
    }
    
    public override void Process(float delta)
    {
        if (_guard == null || _guard.Player == null) return;

        _guard.ChasePlayer(delta);

        if (_guard.Position.DistanceTo(_guard.Player.Position) < 140f)
        {
            fsm.TransitionTo("Attack");
        }
    }

}
