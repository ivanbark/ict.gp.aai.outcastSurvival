using Godot;
using OutCastSurvival.Entities;
using System;
using OutCastSurvival.Entities;

public partial class GoldChest : Item
{
  private int _goldAmount;

  public override void _Ready()
  {
    base._Ready();
    // randomize the gold amount between 8 and 25
    _goldAmount = Math.Abs((int)GD.Randi() % 18) + 8;
  }

  protected override void Collect()
  {
    Player.CollectGold(_goldAmount);
    QueueFree();
  }
}
