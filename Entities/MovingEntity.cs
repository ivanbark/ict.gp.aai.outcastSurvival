using Godot;
using System;
namespace OutCastSurvival.Entities
{
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
    public bool IsAttacking
    {
      get { return _isAttacking; }
      set
      {
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

      World_ref.debug_ref.DebugOptionChanged += InitializeDebugInfo;

      animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
      animatedSprite.Play("right");

      _acceleration = MaxForce / Mass;
      _heading = Vector2.Right; // Initialize heading to face right

      CurrentHealth = MaxHealth;

      InitializeDebugInfo();
    }

    public void InitializeDebugInfo()
    {
      // Get debug info references with null checks
      if (HasNode("DebugInfo"))
      {
        _debugInfo = GetNode<Node2D>("DebugInfo");
        if (_debugInfo != null && World_ref.debug_ref.ShowInfoBox)
        {
          _stateLabel = GetNodeOrNull<Label>("DebugInfo/State");
          _healthLabel = GetNodeOrNull<Label>("DebugInfo/Health");
        }
        _debugInfo.Visible = World_ref.debug_ref.ShowInfoBox && World_ref.debug_ref.ShowDebug;
      }
    }

    public override void _Process(double delta)
    {
      if (Engine.TimeScale == 0f)
        return;

      // Check if next position would be on an obstacle before moving
      if (World_ref?.graph_ref != null && !(this is Sheep) && Velocity != Vector2.Zero)
      {
        Vector2 nextPosition = Position + Velocity * (float)delta;

        // Convert position to graph coordinates
        int tileSize = World_ref.graph_ref.TileSize;
        Vector2I graphCoords = new Vector2I(
          (int)(nextPosition.X / tileSize),
          (int)(nextPosition.Y / tileSize)
        );

        // Check if the tile is an obstacle
        foreach (Obstacle obstacle in World_ref.graph_ref.obstacles)
        {
          if (obstacle.vertex.position == graphCoords)
          {
            // If next position is on an obstacle, prevent movement
            Velocity = Vector2.Zero;
            return;
          }
        }
      }

      // Update animation based on velocity
      if (!_isAttacking)
      {
        if (Velocity != Vector2.Zero)
        {
          var animation = Mathf.Abs(Velocity.X) > Mathf.Abs(Velocity.Y)
            ? Velocity.X > 0 ? "right" : "left"
            : Velocity.Y > 0 ? "down" : "up";
          animatedSprite.Play(animation);
          animatedSprite.FlipH = false;
        }
      }
      else
      {
        animatedSprite.Play("attack");
        animatedSprite.FlipH = _heading.X < 0;
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
      if (World_ref.debug_ref.ShowDebug && _debugInfo != null && World_ref.debug_ref.ShowInfoBox)
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

    public virtual void TakeDamage(int damage)
    {
      CurrentHealth -= damage;
      if (CurrentHealth <= 0)
        Die();
    }

    protected virtual void Die()
    {
      QueueFree();
    }

    public Vector2 Heading
    {
      get { return _heading; }
      private set { _heading = value; }
    }

    protected void UpdateHeading()
    {
      if (Velocity.Length() > 0.1f)
      {
        _heading = Velocity.Normalized();
      }
    }
  }
}
