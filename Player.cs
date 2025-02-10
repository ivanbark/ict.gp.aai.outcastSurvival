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


    UpdateHeading();

    Position += Velocity * (float) delta;

    var animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
    if(Input.IsActionPressed("move_up")) {
      Position += Vector2.Up * 10;
      // animatedSprite2D.Animation = "up";
      // Rotation = (float)Math.PI;
    }

    if(Input.IsActionPressed("move_down")) {
      Position += Vector2.Down * 10;
      // Rotation = (float)Math.PI;
    }

    if(Input.IsActionPressed("move_left")) {
      Position += Vector2.Left * 10;
      // Rotation = (float)Math.PI;
    }

    if(Input.IsActionPressed("move_right")) {
      Position += Vector2.Right * 10;
      // Rotation = (float)Math.PI;
    }
    QueueRedraw();
  }
    public override void _Draw()
    {
        base._Draw();

        DrawLine(Position, Heading, Colors.Blue, 3);
    }
}
