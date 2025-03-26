using Godot;
using System;
using Detection;

public partial class Guard : MovingEntity
{
    private Node2D _player;
    private Vector2 _lastKnownPlayerPosition;
    private GuardDetectionSystem _detectionSystem;

    [Export]
    public float BaseDetectionRange = 200f;
    [Export]
    public float VisionAngle = 90f;
    [Export]
    public float VisionRange = 300f;

    public Vector2 LastKnownPlayerPosition
    {
        get => _lastKnownPlayerPosition;
        set => _lastKnownPlayerPosition = value;
    }

    public Node2D Player
    {
        get => _player;
        set => _player = value;
    }

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

        // Initialize detection system
        _detectionSystem = new GuardDetectionSystem(this, BaseDetectionRange, VisionAngle, VisionRange);
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

        if (_player != null)
        {
            // Check for player detection
            if (_detectionSystem.CanDetectPlayer(_player as Player))
            {
                _detectionSystem.UpdateLastKnownPlayerPosition(_player.Position);
                GetNode<StateMachine.GuardStateMachineNode>("StateMachine").TransitionToAlert();
            }
        }
    }

    public void ChasePlayer(float delta)
    {
        if (_player == null)
            return;

        Vector2 desiredVelocity = SteeringBehaviour.Seek(Position, _player.Position, MaxSpeed);
        ApplyAcceleration(desiredVelocity, delta);
    }

    public void SeekLastKnownPlayerPosition(float delta)
    {
        if (Position.DistanceTo(_lastKnownPlayerPosition) <= 1f)
        {
            Velocity = Vector2.Zero;
            return;
        }

        Vector2 desiredVelocity = SteeringBehaviour.Arrive(Position, _lastKnownPlayerPosition, MaxSpeed, 30);
        ApplyAcceleration(desiredVelocity, delta);
    }

    public void SearchForPlayer(float delta, Vector2 searchPosition)
    {
        if (Position.DistanceTo(searchPosition) <= 1f)
        {
            Velocity = Vector2.Zero;
            return;
        }

        Vector2 desiredVelocity = SteeringBehaviour.Arrive(Position, searchPosition, MaxSpeed, 30);
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
