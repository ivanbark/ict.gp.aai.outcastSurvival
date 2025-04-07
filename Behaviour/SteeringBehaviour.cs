using Godot;
using System;
using System.Collections.Generic;

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

    // path folowing
    public static Vector2 PathFollowing(Vector2 position, List<Vertex> path, int pathIndex)
    {
        if (path == null || path.Count == 0)
        {
            return Vector2.Zero;
        }

        // Get the current and next waypoints
        Vertex currentWaypoint = path[pathIndex];
        Vertex nextWaypoint = path[(pathIndex + 1) % path.Count];

        // Calculate the direction to the next waypoint
        Vector2 direction = (nextWaypoint.position - currentWaypoint.position);
        direction /= direction.Length();

        // Calculate the distance to the next waypoint
        float distanceToNextWaypoint = currentWaypoint.position.DistanceTo(nextWaypoint.position);

        // Calculate the distance to the current position
        float distanceToCurrentPosition = currentWaypoint.position.DistanceTo((Vector2I)position);

        // If we're close to the next waypoint, move to it
        if (distanceToCurrentPosition < distanceToNextWaypoint)
        {
            return Seek(position, currentWaypoint.position, 1.0f);
        }

        // Otherwise, move towards the next waypoint
        return Seek(position, nextWaypoint.position, 1.0f);
    }
}
