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
  public override void _Draw()
  {
    base._Draw();
    
    foreach(var keyval  in graph.graph_collection) 
    {
      Vector2I vec = keyval.Key;
      Vertex vertex = keyval.Value;
      GD.Print(((Vector2)vec * 16) + new Vector2(8,8));
      DrawCircle(graph.MapToLocal(vec), 3, Colors.Red);
      foreach (Edge edge in vertex.neighbors)
      {
        DrawLine(graph.MapToLocal(vertex.position), graph.MapToLocal(edge.to.position),Colors.Red,2);
      }
    }
  }
}
