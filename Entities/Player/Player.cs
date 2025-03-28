using Godot;
using System;
using StateMachine.States;
using StateMachine;

namespace OutCastSurvival.Entities 
{
public partial class Player : MovingEntity
{
  private Vector2 inputDirection = Vector2.Zero;
  public PlayerMovementState CurrentState { get; set; }
  private PlayerStateMachineNode _stateMachineNode;

  [Export]
  public float MaxHunger = 100f;
  public float CurrentHunger;
  private Label _hungerLabel;

  [Export]
  public int BaseHungerDepletionRate = 4;
  private float _hungerDepletionTimer = 0f;
  private float _hungerDepletionInterval = 3f;
  public override void _Ready()
  {
    base._Ready();
    AddToGroup("Entities");
    CurrentHunger = MaxHunger;
    _hungerLabel = GetNode<Label>("DebugInfo/Hunger");
    _stateMachineNode = GetNode<PlayerStateMachineNode>("StateMachine");
    _hungerDepletionTimer = _hungerDepletionInterval;
  }

  public override void _Process(double delta)
  {
    base._Process(delta);

    DepleteHunger((float)delta);

    if (World_ref.visualize_debug_info)
    {
      UpdateHungerLabel();
    }
  }

  private void DepleteHunger(float delta)
  {
    if (Velocity.Length() > 0 || CurrentState == _stateMachineNode.GetStateMachine().GetState<PlayerHungryState>())
    {
      _hungerDepletionTimer -= delta;
      if (_hungerDepletionTimer <= 0f)
      {
        DecreaseHunger(BaseHungerDepletionRate * CurrentState.GetHungerDepletionMultiplier());
        _hungerDepletionTimer = _hungerDepletionInterval;
      }
    }
  }

  private void UpdateHungerLabel()
  {
    if (_hungerLabel != null)
    {
      _hungerLabel.Text = $"Hunger: {CurrentHunger}/{MaxHunger}";
    }
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

  public void DecreaseHunger(float amount)
  {
    if (CurrentHunger == 0)
    {
      TakeDamage(20);
      return;
    }

    CurrentHunger -= amount;
    CurrentHunger = MathF.Round(CurrentHunger, 1);
    if (CurrentHunger < 0)
    {
      CurrentHunger = 0;
    }
  }

  public void Eat(int amount)
  {
    CurrentHunger += amount;
    if (CurrentHunger > MaxHunger)
    {
      CurrentHunger = MaxHunger;
    }

    if (CurrentHunger > MaxHunger * 0.2f)
    {
      _stateMachineNode.SetHungry(false);
    }
  }
}
}