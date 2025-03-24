using Godot;
using System;
using OutCastSurvival.Entities;

namespace OutCastSurvival 
{
public partial class Herd : Node
{
  
  [Export]
  public World World_ref { get; set; }
  [Export]
  private int numberOfChildren = 10;

  [Export]
  private Vector2 startPostionOfHerd;
  public override void _Ready()
  {
    base._Ready();
    for (int i = 0; i < numberOfChildren; i++)
    {
      Sheep child = new()
      {
          World_ref = World_ref,
          Position = startPostionOfHerd
      };
      AddChild(child);
    }
  }
}
}

