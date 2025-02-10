using Godot;
using System;

public partial class Guard : MovingEntity
{
    private float reactionTime = 0.1f;
    private float reactionTimer = 0f;
    private Vector2 lastSeenPosition;
    private Player player;

    public override void _Ready()
    {
        base._Ready();
        AddToGroup("Entities");

        player = GetTree().GetFirstNodeInGroup("Entities") as Player;
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        
        if (player == null) return;

        reactionTimer += (float)delta;
        if (reactionTimer >= reactionTime)
        {
            lastSeenPosition = player.Position;
            reactionTimer = 0f;
        }

        SeekPlayer(lastSeenPosition, (float)delta);
    }

    private void SeekPlayer(Vector2 targetPosition, float delta)
    {
        Vector2 desiredVelocity = SteeringBehaviour.Seek(Position, targetPosition, MaxSpeed);
        ApplyAcceleration(desiredVelocity, delta);

        if (Position.DistanceTo(targetPosition) < 190f)
        {
            GD.Print("STOP!");
            velocity = Vector2.Zero;
        }
    }
}