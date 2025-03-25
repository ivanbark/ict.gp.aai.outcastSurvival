using Godot;
using System;

public partial class Guard : MovingEntity
{
    private Node2D _player;
    public Node2D Player
    {
        get => _player;
        set => _player = value;
    }

    public AnimatedSprite2D AnimatedSprite { get; private set; }

    [Export]
    public float AttackCooldown;
    private float _attackCooldown;

    public override void _Ready()
    {
        base._Ready();

        AddToGroup("Entities");

        if (World_ref != null)
            _player = World_ref.GetNode<Player>("Player");

        _attackCooldown = 0f;
        AnimatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
    }

    public void ChasePlayer(float delta)
    {
        if (_player == null)
            return;

        Vector2 desiredVelocity = SteeringBehaviour.Seek(Position, _player.Position, MaxSpeed);
        ApplyAcceleration(desiredVelocity, delta);
    }

    public void AttackPlayer(float delta)
    {
        if (_player == null)
            return;

        if (Position.DistanceTo(_player.Position) <= AttackRange && _attackCooldown <= 0f)
        {
            GD.Print("Attacking");
            _attackCooldown = AttackCooldown;
            if (_player is Player playerNode)
            {
                playerNode.TakeDamage(AttackDamage);
            }
        }

        _attackCooldown -= delta;
    }
}
