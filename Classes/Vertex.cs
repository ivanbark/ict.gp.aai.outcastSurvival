using System.Collections.Generic;
using Godot;

public class Vertex {

  public Vector2I position;

  public List<Edge> neighbors = new();

  public Vertex(Vector2I position)
  {
    this.position = position;
  }

  public void AddNeighbor(Edge neighbor) {
    if (!neighbors.Contains(neighbor))
      neighbors.Add(neighbor);
  }
}