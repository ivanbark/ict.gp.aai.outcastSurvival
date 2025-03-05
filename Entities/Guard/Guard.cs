using Godot;
using System;

public partial class Guard : MovingEntity
{
    private Player player;

    public override void _Ready()
    {
        base._Ready();
        AddToGroup("Entities");

        player = World_ref.GetNode<Player>("Player");
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        
        if (player == null) return;

        SeekPlayer(player.Position, (float)delta);
    }

    private void SeekPlayer(Vector2 targetPosition, float delta)
    {
        Vector2 desiredVelocity = SteeringBehaviour.Seek(Position, targetPosition, MaxSpeed);
        ApplyAcceleration(desiredVelocity, delta);

        if (Position.DistanceTo(targetPosition) < 190f) // make thsi the width of itself / 2 and the width of the target / 2
        {
            GD.Print("STOP!");
            Velocity = Vector2.Zero;
        }
    }
}