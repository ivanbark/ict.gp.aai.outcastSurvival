using System;
using System.Data.Common;
using Godot;

public class EuclideanHeuristic : IHeuristicStrategy
{
  private static EuclideanHeuristic _instance { get; set; }

  public float DetermineHeuristicValue(Vertex node, Vertex destination)
  {
    if (node == null || destination == null)
      return Mathf.Inf;
    // (dx^2 + dy^2)^0.5
    float dx = MathF.Abs(node.position.X - destination.position.X);
    float dy = MathF.Abs(node.position.Y - destination.position.Y);
    return (dx * dx + dy * dy) * 100;
  }

  private EuclideanHeuristic()
  {

  }

  public static EuclideanHeuristic Instance()
  {
    _instance ??= new();
    return _instance;
  }

}