using Godot;
using System;

public partial class MovingEntity : BaseGameEntity
{
  private Vector2 _heading;
  public AnimatedSprite2D animatedSprite;
  protected Node2D _debugInfo;
  protected Label _stateLabel;
  protected Label _healthLabel;

  [Export]
  public int MaxHealth = 100;
  public int CurrentHealth;

  [Export]
  public int AttackDamage = 25;
  [Export]
  public int AttackRange = 20;
  private bool _isAttacking = false;
  public bool IsAttacking {
    get { return _isAttacking; }
    set {
      _isAttacking = value;
    }
  }

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
    animatedSprite.Animation = "right";
    animatedSprite.Play();

    _acceleration = MaxForce / Mass;
    _heading = Vector2.Right; // Initialize heading to face right

    CurrentHealth = MaxHealth;

    // Get debug info references
    _debugInfo = GetNode<Node2D>("DebugInfo");
    _stateLabel = GetNode<Label>("DebugInfo/State");
    _healthLabel = GetNode<Label>("DebugInfo/Health");

    if (_debugInfo != null)
    {
      _debugInfo.Visible = World_ref.visualize_debug_info;
    }
  }

  public override void _Process(double delta)
  {
    if (Engine.TimeScale == 0f)
      return;

    // Update animation based on velocity
    if (!_isAttacking)
    {
      string direction = Mathf.Abs(Velocity.X) > Mathf.Abs(Velocity.Y)
        ? (Velocity.X > 0 ? "right" : "left")
        : (Velocity.Y > 0 ? "down" : "up");
      animatedSprite.Animation = direction;
      animatedSprite.FlipH = false;
    } else {
      animatedSprite.Animation = "attack";
      animatedSprite.FlipH = Velocity.X < 0;
    }

    // Update heading based on actual movement direction
    if (Velocity.Length() > 0.1f)
    {
      _heading = Velocity.Normalized();
      Rotation = Velocity.Angle();
    }
    else
    {
      // When not moving, maintain the last heading direction
      Rotation = _heading.Angle();
    }

    animatedSprite.GlobalRotation = 0;

    Velocity = Velocity.LimitLength(MaxSpeed);
    Position += Velocity * (float)delta;

    // Counter-rotate the debug info to keep it unrotated
    if (_debugInfo != null)
    {
      _debugInfo.Rotation = -Rotation;
    }

    // Update debug info
    if (World_ref.visualize_debug_info && _debugInfo != null)
    {
      UpdateDebugInfo();
    }
  }

  protected virtual void UpdateDebugInfo()
  {
    // Update state label
    if (_stateLabel != null)
    {
      _stateLabel.Text = $"State: {GetCurrentStateName()}";
    }

    // Update health label
    if (_healthLabel != null)
    {
      _healthLabel.Text = $"HP: {CurrentHealth}/{MaxHealth}";
    }
  }

  protected virtual string GetCurrentStateName()
  {
    return "Unknown";
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
    if (Velocity.Length() > 0.1f)
    {
      _heading = Velocity.Normalized();
    }
  }

  public override void _UnhandledInput(InputEvent @event) {
    if (@event.IsActionPressed("visualize_debug_info")) {
        _debugInfo.Visible = !_debugInfo.Visible;
    }
  }
}

