using System;
using System.Data.Common;

public class EuclideanHeuristic : IHeuristicStrategy
{
  private static EuclideanHeuristic _instance {get; set;}
  
  public float DetermineHeuristicValue(Vertex node, Vertex destination)
  {
    // (dx^2 + dy^2)^0.5
    float dx = MathF.Abs(node.position.X - destination.position.X);
    float dy = MathF.Abs(node.position.Y - destination.position.Y);
    return dx * dx + dy * dy;
  }

  private EuclideanHeuristic() {

  }

  public static EuclideanHeuristic Instance()
  {
    _instance ??= new ();
    return _instance;
  }
  
}