using Godot;
using System;

public partial class MovingEntity : BaseGameEntity
{
  private Vector2 _heading;
  public AnimatedSprite2D animatedSprite;

  [Export]
  public int MaxHealth = 100;
  public int CurrentHealth = 100;

  [Export]
  public int AttackDamage = 25;
  [Export]
  public int AttackRange = 20;

  [Export]
  public int MaxSpeed { get; set; } = 150;

  [Export] public int MaxForce { get; set; } = 217;

  [Export]
  public float Mass { get; set; } = 1.5f;

  private float _acceleration = 0f;

  public override void _Ready()
  {
    base._Ready();

    animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");

    _acceleration = MaxForce / Mass;
  }

  public override void _Process(double delta)
  {
    // GD.Print($"MovingEntity _Process - Velocity: {Velocity}");

    if (Mathf.Abs(Velocity.X) > Mathf.Abs(Velocity.Y))
    {
      if (Velocity.X > 0)
      {
        animatedSprite.Animation = "right";
      }
      else if (Velocity.X < 0)
      {
        animatedSprite.Animation = "left";
      }
    }
    else if (Velocity.Y > 0)
    {
      animatedSprite.Animation = "down";
    }
    else if (Velocity.Y < 0)
    {
      animatedSprite.Animation = "up";
    }

    Rotation = Velocity.Angle();
    animatedSprite.GlobalRotation = 0;

    Velocity = Velocity.LimitLength(MaxSpeed);
    Position += Velocity * (float)delta;
  }

  public void ApplyAcceleration(Vector2 desiredVelocity, float delta)
  {
    float sharpTurnFactor = 2.0f; // Increase this factor for sharper turns
    float decelerationFactor = 0.85f; // Adjust for smoother stops

    if (Velocity.Dot(desiredVelocity) < 0)
    {
      Velocity = desiredVelocity.Normalized() * Mathf.Max(_acceleration * delta * sharpTurnFactor, desiredVelocity.Length() * 0.5f);
    }
    else
    {
      Velocity += (desiredVelocity - Velocity).Normalized() * _acceleration * delta * sharpTurnFactor;
    }

    // Only apply deceleration if both current and desired velocities are very small
    if (Velocity.Length() < 10f && desiredVelocity.Length() < 10f)
    {
      Velocity *= decelerationFactor;
    }
  }

  public void TakeDamage(int damage)
  {
    CurrentHealth -= damage;
    if (CurrentHealth <= 0)
    {
      Die();
    }
  }

  protected virtual void Die()
  {
    QueueFree();
  }

  public Vector2 Heading {
    get { return _heading; }
    private set { _heading = value; }
  }

  protected void UpdateHeading() {
    Heading = new Vector2(Velocity.X, Velocity.Y).Normalized();
  }
}

