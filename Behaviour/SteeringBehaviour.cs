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

    public static Vector2 Arrive(Vector2 position, Vector2 targetPosition, float maxSpeed, float slowingRadius)
    {
        Vector2 toTarget = targetPosition - position;
        float distance = toTarget.Length();
        
        if (distance < 0.1f)
        {
            return Vector2.Zero;
        }

        float targetSpeed;
        if (distance > slowingRadius)
        {
            targetSpeed = maxSpeed;
        }
        else
        {
            targetSpeed = maxSpeed * (distance / slowingRadius);
        }

        Vector2 desiredVelocity = toTarget.Normalized() * targetSpeed;
        Vector2 steeringForce = desiredVelocity;

        if (distance < slowingRadius * 0.5f)
        {
            steeringForce *= 0.5f;
        }

        float steeringForceLength = steeringForce.Length();
        if (steeringForceLength > maxSpeed)
        {
            steeringForce = steeringForce.Normalized() * maxSpeed;
        }

        return steeringForce;
    }
}
