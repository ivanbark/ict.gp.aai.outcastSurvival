using Godot;
using OutCastSurvival;
using OutCastSurvival.Entities;
using System;
using System.Collections.Generic;

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
        foreach (Vertex vertex in world_ref.graph_ref.vertices)
        {
          if (vertex.Visited)
            DrawCircle(world_ref.graph_ref.MapToLocal(vertex.position), 2, Colors.Yellow);
        }

        foreach (Edge edge in world_ref.graph_ref.edges)
        {
          DrawLine(world_ref.graph_ref.MapToLocal(edge.from.position), world_ref.graph_ref.MapToLocal(edge.to.position), Colors.Yellow, 1);
        }

      }


      if (world_ref.debug_ref.ShowPaths)
      {
        if (world_ref.TargetVertex != null)
        {
          Vector2 start = new(world_ref.TargetVertex.position.X * world_ref.graph_ref.TileSize + world_ref.graph_ref.TileSize / 2, world_ref.TargetVertex.position.Y * world_ref.graph_ref.TileSize + world_ref.graph_ref.TileSize / 2);
          DrawCircle(start, 5, Colors.Black);
        }

        var allsheep = GetTree().GetNodesInGroup("Sheep");
        foreach (Node entity in allsheep)
        {
          if (entity is Sheep sheep)
          {
            List<Vertex> path = sheep.path;
            if (path != null)
            {
              GD.Print("Path: " + path.Count);
              for (int i = sheep.pathIndex; i < path.Count - 1; i++)
              {
                Vector2 start = new(path[i].position.X, path[i].position.Y);
                Vector2 end = new(path[i + 1].position.X, path[i + 1].position.Y);
                DrawLine(start, end, Colors.RoyalBlue, 5);
                DrawCircle(start, 5, Colors.RoyalBlue);
              }
            }
          }
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
