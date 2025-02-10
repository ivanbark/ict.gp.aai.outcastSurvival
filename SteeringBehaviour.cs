using Godot;
using System;

public partial class SteeringBehaviour : Node
{
    public static Vector2 Seek(Vector2 position, Vector2 targetPosition, float maxSpeed)
    {
        return (targetPosition - position).Normalized() * maxSpeed;
    }
}
