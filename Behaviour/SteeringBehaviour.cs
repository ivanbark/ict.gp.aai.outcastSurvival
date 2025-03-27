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

        // If we're very close to the target, stop
        if (distance < 0.1f)
        {
            return Vector2.Zero;
        }

        // Calculate the target speed based on distance
        float targetSpeed;
        if (distance > slowingRadius)
        {
            targetSpeed = maxSpeed;
        }
        else
        {
            // Smoothly reduce speed as we get closer
            targetSpeed = maxSpeed * (distance / slowingRadius);
        }

        // Calculate the desired velocity
        Vector2 desiredVelocity = toTarget.Normalized() * targetSpeed;

        // Calculate the steering force
        Vector2 steeringForce = desiredVelocity;

        // Apply stronger braking when close to target
        if (distance < slowingRadius * 0.5f)
        {
            steeringForce *= 0.5f;
        }

        // Clamp the steering force to prevent sudden movements
        float steeringForceLength = steeringForce.Length();
        if (steeringForceLength > maxSpeed)
        {
            steeringForce = steeringForce.Normalized() * maxSpeed;
        }

        return steeringForce;
    }
}
