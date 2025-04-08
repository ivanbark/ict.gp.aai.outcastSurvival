using System.Collections.Generic;
using System.Dynamic;
using System.Runtime.CompilerServices;
using Godot;

public class Vertex
{

  public Vector2I position;

  public bool isDiagonal;
  public List<Edge> neighbors = [];

  public Vertex A_Star_previous;
  public bool A_Star_visited;
  public float A_Star_distance;

  private bool _enqueued = false;
  private bool _visited = false;
  public bool Visited
  {
    get { return _visited; }
    private set { _visited = value; }
  }
  public bool Enqueued
  {
    get { return _enqueued; }
    private set { _enqueued = value; }
  }

  public void Visit()
  {
    Visited = true;
  }

  public void Enqueue()
  {
    Enqueued = true;
  }

  public void Reset_A_Star()
  {
    A_Star_previous = null;
    A_Star_distance = -Mathf.Inf;
    A_Star_visited = false;
  }

  public Vertex(Vector2I position, bool diagonal)
  {
    this.position = position;
    isDiagonal = diagonal;
  }
  public Vertex(Vector2I position) : this(position, false) { }

  public Vertex(Vertex other)
  {
    position = other.position;
    isDiagonal = other.isDiagonal;
    _enqueued = other._enqueued;
    _visited = other._visited;
    neighbors = [.. other.neighbors];
  }

  public void AddNeighbor(Edge neighbor)
  {
    if (!neighbors.Contains(neighbor))
      neighbors.Add(neighbor);
  }

  public override bool Equals(object obj)
  {
    if (obj is Vertex other)
    {
      return position == other.position;
    }
    return false;
  }

  public override int GetHashCode()
  {
    return position.GetHashCode();
  }
  public override string ToString()
  {
    return position.ToString();
  }
}