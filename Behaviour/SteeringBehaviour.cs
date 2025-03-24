using Godot;
using System;

public class SteeringBehaviour
{
    public static Vector2 Seek(Vector2 position, Vector2 targetPosition, float maxSpeed)
    {
        return (targetPosition - position).Normalized() * maxSpeed;
    }
    public static Vector2 Flee(Vector2 position, Vector2 targetPosition, float maxSpeed)
    {
        return (position - targetPosition).Normalized() * maxSpeed;
    }
}
