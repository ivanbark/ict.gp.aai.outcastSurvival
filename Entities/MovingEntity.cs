using Godot;
using System;

public partial class MovingEntity : BaseGameEntity
{
  private Vector2 _heading;

  [Export]
  public int MaxSpeed { get; set; } = 900;

  [Export] public int MaxForce { get; set; } = 1300;

  [Export]
  public int MinSpeed { get; set; } = 0;

  [Export]
  public float Mass { get; set; } = 1.5f;

  [Export] 
  public float Acceleration = 0f;

  public override void _Ready()
  {
    Acceleration = MaxForce / Mass;
  }

  public override void _Process(double delta)
  {
    AnimatedSprite2D sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
    if (Mathf.Abs(Velocity.X) > Mathf.Abs(Velocity.Y))
    {
      if (Velocity.X > 0)
      {
        sprite.Animation = "right";
      }
      else if (Velocity.X < 0)
      {
        sprite.Animation = "left";
      }
    }
    else if (Velocity.Y > 0)
    {
      sprite.Animation = "down";
    }
    else if (Velocity.Y < 0)
    {
      sprite.Animation = "up";
    }

    Rotation = Velocity.Angle();
    sprite.GlobalRotation = 0;
    
    Velocity = Velocity.LimitLength(MaxSpeed);
    Position += Velocity * (float)delta; 
  }
  
  public void ApplyAcceleration(Vector2 desiredVelocity, float delta)
  {
    if (Velocity.Dot(desiredVelocity) < 0)
    {
      Velocity = desiredVelocity.Normalized() * Mathf.Max(Acceleration * delta, desiredVelocity.Length() * 0.5f);
    }
    else
    {
      Velocity += (desiredVelocity - Velocity).Normalized() * Acceleration * delta;
    }
  }

  
  public Vector2 Heading {
    get { return _heading; }
    private set { _heading = value; }
  }

  protected void UpdateHeading() {
    Heading = new Vector2(Velocity.X, Velocity.Y).Normalized();
  }
}

