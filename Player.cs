using Godot;
using System;

public partial class Player : MovingEntity
{

  public override void _Ready()
  {
      base._Ready();
      AddToGroup("Entities");
  }

  public override void _Process(double delta)
  {
    // implement seek for now

    Vector2 DesiredVelocity = (World_ref.Target - Position) * MaxSpeed;
    Vector2 steeringForce = DesiredVelocity - Velocity;

    Vector2 acceleration = steeringForce / Mass;

    Velocity += acceleration * (float) delta;
    // truncate the velocity to the maxspeed


    Position += Velocity * (float) delta;
  }
}
