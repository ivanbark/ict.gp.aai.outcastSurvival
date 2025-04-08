using System;

public class ManhattanHeuristic : IHeuristicStrategy
{
  private static ManhattanHeuristic _instance { get; set; }

  public float DetermineHeuristicValue(Vertex node, Vertex destination)
  {
    // dx + dy
    return MathF.Abs(node.position.X - destination.position.X) + MathF.Abs(node.position.Y - destination.position.Y) * 100;
  }

  private ManhattanHeuristic() { }

  public static ManhattanHeuristic Instance()
  {
    _instance ??= new();
    return _instance;
  }

}