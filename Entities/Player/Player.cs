using Godot;
using System;
using StateMachine.States;
using StateMachine;

public partial class Player : MovingEntity
{
  private Vector2 inputDirection = Vector2.Zero;
  public IState CurrentState { get; set; }

  public override void _Ready()
  {
    base._Ready();
    AddToGroup("Entities");
  }

  protected override string GetCurrentStateName()
  {
    return CurrentState?.GetType().Name ?? "Unknown";
  }

  protected override void Die()
  {
    GD.Print("Player has died. Game Over!");
    GetTree().Paused = true;
  }
}
