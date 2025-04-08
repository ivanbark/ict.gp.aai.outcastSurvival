using Godot;
using OutCastSurvival;
using System;

public partial class GraphDrawer : Node2D
{
  [Export]
  private World world_ref;

  public override void _Ready()
  {
    world_ref.debug_ref.DebugOptionChanged += () => QueueRedraw();
  }

  public override void _Process(double delta)
  {
      base._Process(delta);
  }

  public override void _Draw()
  {
    base._Draw();

    if (world_ref.debug_ref.ShowDebug)
    {
      if (world_ref.debug_ref.ShowGraph)
      {
        foreach(Vertex vertex  in world_ref.graph_ref.vertices)
        {
          if (vertex.Visited)
            DrawCircle(world_ref.graph_ref.MapToLocal(vertex.position), 2, Colors.Yellow);
        }

        foreach (Edge edge in world_ref.graph_ref.edges)
        {
          DrawLine(world_ref.graph_ref.MapToLocal(edge.from.position), world_ref.graph_ref.MapToLocal(edge.to.position),Colors.Yellow,1);
        }

      }

      if (world_ref.debug_ref.ShowObstacles)
      {
        GD.Print(world_ref.graph_ref.obstacles.Count + " obstacles");
        foreach (Obstacle obstacle in world_ref.graph_ref.obstacles)
        {
          DrawCircle(world_ref.graph_ref.MapToLocal(obstacle.vertex.position), 3, Colors.Red);
        }
      }
    }
  }
}
