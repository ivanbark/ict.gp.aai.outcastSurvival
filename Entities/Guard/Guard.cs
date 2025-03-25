using Godot;
using System;

public partial class Guard : MovingEntity
{
    public Player Player;

    [Export]
    public float AttackCooldown;
    private float _attackCooldown;

    public override void _Ready()
    {
        base._Ready();

        AddToGroup("Entities");

        if (World_ref != null)
            Player = World_ref.GetNode<Player>("Player");

        _attackCooldown = 0f;
    }

    public void ChasePlayer(float delta)
    {
        if (Player == null)
            return;

        Vector2 desiredVelocity = SteeringBehaviour.Seek(Position, Player.Position, MaxSpeed);
        ApplyAcceleration(desiredVelocity, delta);
    }

    public void AttackPlayer(float delta)
    {
        if (Player == null)
            return;

        if (Position.DistanceTo(Player.Position) <= AttackRange && _attackCooldown <= 0f)
        {
            GD.Print("Attacking");
            _attackCooldown = AttackCooldown;
            Player.TakeDamage(AttackDamage);
        }

        _attackCooldown -= delta;
    }
}
