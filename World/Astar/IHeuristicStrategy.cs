

public interface IHeuristicStrategy
{
  public float DetermineHeuristicValue(Vertex node, Vertex destination);
  public static IHeuristicStrategy Instance;
}