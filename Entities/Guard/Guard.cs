using Godot;
using System;
using Detection;
using StateMachine;

namespace OutCastSurvival.Entities 
{
public partial class Guard : MovingEntity
{
    private Node2D _player;
    private Vector2 _lastKnownPlayerPosition;
    private GuardDetectionSystem _detectionSystem;
    private GuardStateMachineNode _stateMachineNode;

    [Export]
    public float BaseDetectionRange = 200f;
    [Export]
    public float VisionAngle = 90f;

    // Debug visualization colors
    private readonly Color _baseDetectionColor = new Color(1, 1, 0, 0.2f); // Yellow with transparency
    private readonly Color _visionConeColor = new Color(0, 1, 0, 0.2f); // Green with transparency
    private readonly Color _sideDetectionColor = new Color(1, 0.5f, 0, 0.2f); // Orange with transparency
    private readonly Color _backDetectionColor = new Color(1, 0, 0, 0.2f); // Red with transparency

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
        _detectionSystem = new GuardDetectionSystem(this, BaseDetectionRange, VisionAngle);

        _stateMachineNode = GetNode<GuardStateMachineNode>("StateMachine");
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

        if (_player != null)
        {
            // Check for player detection
            if (_detectionSystem.CanDetectPlayer(_player as Player))
            {
                _lastKnownPlayerPosition = _player.Position;
                _stateMachineNode.TransitionToAlert();
            }
        }

        // Request redraw every frame to update visualization
        QueueRedraw();
    }

    public override void _Draw()
    {
        base._Draw();

        // Get the inverse scale to compensate for the Guard's scale
        Vector2 inverseScale = new Vector2(1f / Scale.X, 1f / Scale.Y);

        if (World_ref.visualize_debug_info) {
            // Draw base detection range
            DrawCircle(Vector2.Zero, BaseDetectionRange * inverseScale.X, _baseDetectionColor);

            // Draw vision cone
            DrawVisionCone(inverseScale);

            // Draw detection ranges based on player state if player exists
            if (_player != null)
            {
                DrawStateBasedDetectionRange(inverseScale);
            }
        }
    }

    private void DrawVisionCone(Vector2 inverseScale)
    {
        // Convert vision angle to radians
        float halfVisionAngle = VisionAngle * Mathf.Pi / 180f / 2f;

        // Calculate number of segments for the arc (more segments = smoother curve)
        const int segments = 32;
        Vector2[] points = new Vector2[segments + 2]; // +2 for center point and closing point

        // Center point
        points[0] = Vector2.Zero;

        // Calculate points along the arc
        for (int i = 0; i <= segments; i++)
        {
            float angle = -halfVisionAngle + (2 * halfVisionAngle * i / segments);
            points[i + 1] = new Vector2(
                BaseDetectionRange * Mathf.Cos(angle),
                BaseDetectionRange * Mathf.Sin(angle)
            );
        }

        // Apply inverse scale to points
        for (int i = 0; i < points.Length; i++)
        {
            points[i] *= inverseScale;
        }

        // Draw the rounded vision cone
        DrawPolygon(points, new Color[] { _visionConeColor });
    }

    private void DrawStateBasedDetectionRange(Vector2 inverseScale)
    {
        if (_player is Player playerNode)
        {
            float detectionRange = _detectionSystem.CalculateDetectionRange(playerNode);
            Vector2 directionToPlayer = (_player.Position - Position).Normalized();
            float angleToPlayer = Mathf.Abs(Rotation - directionToPlayer.Angle());

            // Normalize angle to 0-180 range
            if (angleToPlayer > Mathf.Pi)
            {
                angleToPlayer = 2 * Mathf.Pi - angleToPlayer;
            }

            // Calculate detection multiplier based on angle
            float detectionMultiplier = _detectionSystem.CalculateDetectionMultiplier(angleToPlayer);
            float finalDetectionRange = detectionRange * detectionMultiplier;

            // Draw the detection range with appropriate color based on position
            Color detectionColor = _baseDetectionColor;
            if (angleToPlayer > Mathf.Pi / 2f)
            {
                detectionColor = _backDetectionColor;
            }
            else if (angleToPlayer > VisionAngle * Mathf.Pi / 180f / 2f)
            {
                detectionColor = _sideDetectionColor;
            }

            DrawCircle(Vector2.Zero, finalDetectionRange * inverseScale.X, detectionColor);
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

    protected override string GetCurrentStateName()
    {
        return _stateMachineNode?.GetCurrentState()?.StateName ?? "Unknown";
    }
}
