using Godot;
using System;

public partial class MovingEntity : BaseGameEntity
{
  private Vector2 _heading;


  [Export]
  public int Speed { get; set; } = 400;

  [Export]
  public float RotationSpeed { get; set; } = 1.5f;

  [Export]
  public int MaxSpeed { get; set; } = 1000;

  [Export]
  public int MinSpeed { get; set; } = 0;

  [Export]
  public float MaxForce { get; set; } = 25.0f;

  // position is a property of CharacterBody2D

  //heading is deterimed by the property rotation

  [Export]
  public int Mass { get; set; } = 10;

  // velocity is inherited by property velocity;

  public Vector2 Heading {
    get { return _heading; }
    private set { _heading = value; }
  }

  protected void UpdateHeading() {
    Heading = new Vector2(Velocity.X, Velocity.Y).Normalized();
  }

  public void WrapArround() {
    // Position = new Vector2(
    //   Math.Clamp(Position.X, 0, World_ref.WorldSize.X),
    //   Math.Clamp(Position.Y , 0, World_ref.WorldSize.Y)
    // );
  }
}
