using Godot;
using System;
using StateMachine.States;
using StateMachine;

public partial class Player : MovingEntity
{
  private Vector2 inputDirection = Vector2.Zero;
  public IState CurrentState { get; private set; }

  public override void _Ready()
  {
    base._Ready();

    AddToGroup("Entities");
  }

  public override void _Process(double delta)
  {
    base._Process(delta);
  }

  public override void _Draw()
  {
    base._Draw();
  }

  protected override void Die()
  {
    GD.Print("Player has died. Game Over!");
    GetTree().Paused = true;
  }

  public void SetCurrentState(IState state)
  {
    CurrentState = state;
  }
}
