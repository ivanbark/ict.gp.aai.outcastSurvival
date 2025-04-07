using System.Collections.Generic;
using System.Dynamic;
using System.Runtime.CompilerServices;
using Godot;

public class Obstacle
{
  public Vertex vertex;
  public ObstacleType type;

  public Obstacle(Vertex vertex, ObstacleType type)
  {
    this.vertex = vertex;
    this.type = type;
  }
  public Obstacle(Obstacle obstacle) 
  {
    vertex = new(obstacle.vertex.position);
    type = obstacle.type;
  }

  public override bool Equals(object obj)
  {
    if (obj is Obstacle other) 
    {
      return (
        vertex ==  other.vertex &&
        type == other.type
      );
    }
    return false;
  }

  public override int GetHashCode()
  {
    int hash = 17;
    hash = hash * 31 + (vertex != null ? vertex.GetHashCode() : 0);
    hash = hash * 31 + type.GetHashCode();
    return hash;
  }

  public override string ToString()
  {
    return $"Vertex: {vertex}, Type: {type}";
  }

}