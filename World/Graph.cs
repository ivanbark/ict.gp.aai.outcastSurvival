using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.Marshalling;
using Godot;

public partial class Graph : TileMapLayer
{
  [Export]
  private Vector2I GraphOrigin = new(50, 50);

  [Export]
  private bool USING_DIAGONALS = false;

  public HashSet<Edge> edges = [];
  public HashSet<Vertex> vertices = [];

  private readonly Queue<Vertex> vertex_queue = [];

  // Called when the node enters the scene tree for the first time.
  public override void _Ready()
  {
     GenerateGraphBFS();
  }

  // Called every frame. 'delta' is the elapsed time since the previous frame.
  public override void _Process(double delta)
  {
    // QueueRedraw();
  }

  public Vertex GetVertexForPosition(Vector2I position)
  {
    Vertex vert = ReadOnlyTryGetValue(new Vertex(position));
    return vert;
  }


  private void GenerateGraphBFS()
  {
    //set all distances to Infinity
    // done by setting vertex.Enqueued to false by default.

    //set distance at start vertex to 0 and Enqueue the start vertex
    if(!TryGetVertexFromHashSet(new (GraphOrigin), out Vertex start_vertex)) 
    {
      return;// not a valid starting vertex
    }
    start_vertex.Enqueue(); // set to equeued for duplication prevention
    vertex_queue.Enqueue(start_vertex);

    //While queue not empty
    while(vertex_queue.Count != 0) 
    {
      // Vertex = dequeue
      Vertex current = vertex_queue.Dequeue();
      current.Visit(); // another duplication prevention

      //for each neighbor do:
      Vertex[] neighbors = GetNeighboringCells(current);
      foreach (Vertex neighbor in neighbors) 
      {
        // this neighbor is valid and in the vertices hashset
        
        // make a connection with this neighbor
        edges.Add(new Edge(current, neighbor, neighbor.isDiagonal? 141: 100)); //141 form diagonal and 100 for a straight line

        // enqueue this neighbor if not enqueued already
        if(!neighbor.Enqueued)
        {
          neighbor.Enqueue();
          vertex_queue.Enqueue(neighbor);
        }
      }
    }

  }
  private Vertex[] GetNeighboringCells(Vertex referenceCell)
  {
    List<Vertex> neighbors = [];

    // straight paths
    // down
    if (TryGetVertexFromHashSet(new(new (referenceCell.position.X, referenceCell.position.Y + 1)), out Vertex down_vertex)) 
      neighbors.Add(down_vertex);
    // up
    if (TryGetVertexFromHashSet(new(new (referenceCell.position.X, referenceCell.position.Y - 1)), out Vertex up_vertex)) 
      neighbors.Add(up_vertex);
    // left
    if (TryGetVertexFromHashSet(new(new (referenceCell.position.X - 1, referenceCell.position.Y)), out Vertex left_vertex)) 
      neighbors.Add(left_vertex);
    // right
    if (TryGetVertexFromHashSet(new(new (referenceCell.position.X + 1, referenceCell.position.Y)), out Vertex right_vertex)) 
      neighbors.Add(right_vertex);

    // diagonals the cost should be higher
    if (USING_DIAGONALS) 
    {
      // top left
      if (TryGetVertexFromHashSet(new(new (referenceCell.position.X - 1, referenceCell.position.Y - 1), true), out Vertex top_left_vertex)) 
        neighbors.Add(top_left_vertex);
      // top right
      if (TryGetVertexFromHashSet(new(new (referenceCell.position.X + 1, referenceCell.position.Y - 1), true), out Vertex top_right_vertex)) 
        neighbors.Add(top_right_vertex);
      // bottom left
      if (TryGetVertexFromHashSet(new(new (referenceCell.position.X - 1, referenceCell.position.Y + 1), true), out Vertex bottom_left_vertex)) 
        neighbors.Add(bottom_left_vertex);
      // bottom right
      if (TryGetVertexFromHashSet(new(new (referenceCell.position.X + 1, referenceCell.position.Y + 1), true), out Vertex bottom_right_vertex)) 
        neighbors.Add(bottom_right_vertex);
    }
    return [.. neighbors];
  }

  private bool TryGetVertexFromHashSet(Vertex current, out Vertex output) 
  {
    if (GetCellSourceId(current.position) != 0)
    {
      output = null;
      return false;
    }

    if (vertices.TryGetValue(current, out Vertex actualVertex)) 
    {
      output = actualVertex;
    } 
    else 
    {
      vertices.Add(current);
      output = current;
    }
    return true;
  } 

  private bool ReadOnlyTryGetValue(Vertex probe, out Vertex output) 
  {
    if (vertices.TryGetValue(probe, out Vertex actualVertex)) 
    {
      output = actualVertex;
    } 
    else 
    {
      output = probe;
    }
    return true;
  }

  public override void _Draw()
  {
    base._Draw();
  }
}
