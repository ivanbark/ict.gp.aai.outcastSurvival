using Godot;
using System;

public partial class MovingEntity : BaseGameEntity
{
  private Vector2 _heading;

  [Export]
  public int MaxSpeed { get; set; } = 400;

  [Export]
  public int MinSpeed { get; set; } = 0;

  [Export]
  public float MaxForce { get; set; } = 600;

  [Export]
  public int Mass { get; set; } = 2;

  [Export] 
  public float Acceleration = 0f;

  protected Vector2 velocity = Vector2.Zero;

  public override void _Ready()
  {
    Acceleration = MaxForce / Mass;
  }

  public override void _Process(double delta)
  {
    AnimatedSprite2D sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
    if (Mathf.Abs(velocity.X) > Mathf.Abs(velocity.Y))
    {
      if (velocity.X > 0)
      {
        sprite.Animation = "right";
      }
      else if (velocity.X < 0)
      {
        sprite.Animation = "left";
      }
    }
    else if (velocity.Y > 0)
    {
      sprite.Animation = "down";
    }
    else if (velocity.Y < 0)
    {
      sprite.Animation = "up";
    }

    Rotation = velocity.Angle();
    sprite.GlobalRotation = 0;
  }
  
  public override void _PhysicsProcess(double delta)
  {
    velocity = velocity.LimitLength(MaxSpeed);
    Position += velocity * (float)delta; 
    MoveAndSlide(); 
  }
  
  public void ApplyAcceleration(Vector2 desiredVelocity, float delta)
  {
    Vector2 newVelocity = (desiredVelocity - velocity).Normalized() * Acceleration * delta;
    velocity += newVelocity;
  }
  
  public Vector2 Heading {
    get { return _heading; }
    private set { _heading = value; }
  }

  protected void UpdateHeading() {
    Heading = new Vector2(Velocity.X, Velocity.Y).Normalized();
  }
}

