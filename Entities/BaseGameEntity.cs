using Godot;
using System;
namespace OutCastSurvival.Entities 
{
public partial class BaseGameEntity : CharacterBody2D
{
  [Export]
  public bool visualize_debug_info { get; set; } = false;

  [Export]
  public World World_ref { get; set; }

  // Called when the node enters the scene tree for the first time.
  public override void _Ready()
  {
    World_ref = GetTree().Root.GetNodeOrNull<World>("World");
  }

  // Called every frame. 'delta' is the elapsed time since the previous frame.
  public override void _Process(double delta)
  {
    if (Engine.TimeScale == 0f)
      return;
  }
}
}