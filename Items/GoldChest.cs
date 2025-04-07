using Godot;
using OutCastSurvival.Entities;
using System;

public partial class GoldChest : Area2D
{
  private int _goldAmount = 10;

  public override void _Ready()
  {
    // randomize the gold amount between 8 and 25
    _goldAmount = Math.Abs((int)GD.Randi() % 18) + 8;
    BodyEntered += OnBodyEntered;
  }

  public void OnBodyEntered(Node2D body)
  {
    if (body is Player player)
    {
      player.CollectGold(_goldAmount);
      QueueFree();
    }
  }

}
