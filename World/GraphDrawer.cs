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
    
    foreach(Vertex vertex  in graph.vertices) 
    {
      if (vertex.Visited)
        DrawCircle(graph.MapToLocal(vertex.position), 2, Colors.Red);
    }

    foreach (Edge edge in graph.edges)
    {
      DrawLine(graph.MapToLocal(edge.from.position), graph.MapToLocal(edge.to.position),Colors.Red,1);
    }
  }
}
