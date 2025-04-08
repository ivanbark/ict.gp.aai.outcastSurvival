using Godot;
using System;
using OutCastSurvival.Entities;
public partial class Meat : Item
{
  private int _meatAmount;

  public override void _Ready()
  {
    base._Ready();
    // randomize the gold amount between 8 and 25
    _meatAmount = Math.Abs((int)GD.Randi() % 18) + 8;
  }

  protected override void Collect()
  {
    Player.Eat(_meatAmount);
    QueueFree();
  }
}
