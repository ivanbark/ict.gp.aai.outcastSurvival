using Godot;
using System;

public partial class Player : MovingEntity
{ 
  private Vector2 inputDirection = Vector2.Zero;

  public override void _Ready()
  {
    base._Ready();
    AddToGroup("Entities");
  }

  public override void _Process(double delta)
  {
    // TODO: make use of physics from MovingEntity
    
    var animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
    
    if(Input.IsActionPressed("move_up")) {
      Position += Vector2.Up * 10;
      animatedSprite2D.Animation = "up";
      Rotation = (float)Math.PI;
      animatedSprite2D.GlobalRotation = 0;
    }
  
    if(Input.IsActionPressed("move_down")) {
      Position += Vector2.Down * 10;
      animatedSprite2D.Animation = "down";
      Rotation = (float)Math.PI;
      animatedSprite2D.GlobalRotation = 0;
    }
  
    if(Input.IsActionPressed("move_left")) {
      Position += Vector2.Left * 10;
      animatedSprite2D.Animation = "left";
      Rotation = (float)Math.PI;
      animatedSprite2D.GlobalRotation = 0;
    }
  
    if(Input.IsActionPressed("move_right")) {
      Position += Vector2.Right * 10;
      animatedSprite2D.Animation = "right";
      Rotation = (float)Math.PI;
      animatedSprite2D.GlobalRotation = 0;
    }
    
    QueueRedraw();
  }
  
    public override void _Draw()
    {
      // Line to target
      // DrawLine(Position, World_ref.Target, Colors.Blue, 1);

      base._Draw();

      // if(visualize_debug_info) {

      // // heading is incorrect
      // DrawLine(Position, Heading, Colors.Red, 1);
      // }

    }
}
