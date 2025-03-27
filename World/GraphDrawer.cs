using Godot;
using System;

public partial class GraphDrawer : Node2D
{

  [Export]
  public bool draw_graph = true;

  [Export]
  private Graph graph;

  public override void _Ready()
  {
  }

  public override void _Process(double delta)
  {
      base._Process(delta);
      if (Input.IsActionJustPressed("toggle_display_graph")) {
        GD.Print("Toggle Graph");
        draw_graph = !draw_graph;
        QueueRedraw();
      }
      
    
  }
  public override void _Draw()
  {
    base._Draw();
    if (draw_graph) 
    {
      foreach(Vertex vertex  in graph.vertices) 
      {
        if (vertex.Visited)
          DrawCircle(graph.MapToLocal(vertex.position), 2, Colors.Yellow);
      }

      foreach (Edge edge in graph.edges)
      {
        DrawLine(graph.MapToLocal(edge.from.position), graph.MapToLocal(edge.to.position),Colors.Yellow,1);
      }
      GD.Print(graph.obstacles.Count + " obstacles");
      foreach (Obstacle obstacle in graph.obstacles)
      {
        DrawCircle(graph.MapToLocal(obstacle.vertex.position), 3, Colors.Red);
      }
    }
  }
}
