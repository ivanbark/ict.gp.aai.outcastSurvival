using Godot;
using System;
using OutCastSurvival.Entities;

namespace OutCastSurvival 
{
public partial class Herd : Node
{

  [Export]

  private PackedScene SheepScene;

  [Export]
  private int numberOfChildren = 10;

  [Export]
  private Vector2 startPostionOfHerd;
  public override void _Ready()
  {
    base._Ready();
    for (int i = 0; i < numberOfChildren; i++)
    {
      Sheep sheep = SheepScene.Instantiate<Sheep>();
      sheep.Position = startPostionOfHerd + new Vector2(GD.Randf() * 10, GD.Randf() * 10);
      AddChild(sheep);
    }
  }
}
}

