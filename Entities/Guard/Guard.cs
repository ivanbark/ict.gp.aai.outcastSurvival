using Godot;
using System;

namespace OutCastSurvival.Entities 
{
public partial class Guard : MovingEntity
{
    public Player Player;
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
        
        GD.Print("start attacking");
        
        if (Position.DistanceTo(Player.Position) < 150f && animatedSprite.Frame == 2 && _attackCooldown <= 0f)
        {
            _attackCooldown = .75f;
            Player.TakeDamage(25);
            GD.Print("Attacking");
        }
        
        _attackCooldown -= delta;
        GD.Print("stop attacking");
    }
}
}