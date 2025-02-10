using Godot;
using System;

public partial class World : Node
{
  [Export]
  // maybe collision box arround the screen?
  public Vector2 WorldSize { get; set; } = new(200,200);

  [Export]
  public Vector2 Target { get; set; } = new(500, 500);

  public override void _Process(double delta)
  {
    // foreach(Node entity in GetTree().GetNodesInGroup("Entities")) {
    //   if (entity is MovingEntity me) {
    //     me.WrapArround();
    //   }
    // }
  }
}
