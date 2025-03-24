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

  public HashSet<Edge> edges = [];
  public HashSet<Vertex> vertices = [];

  private readonly Queue<Vertex> vertex_queue = [];

  // Called when the node enters the scene tree for the first time.
  public override void _Ready()
  {
    if (GetCellSourceId(GraphOrigin) == -1)
      return;

    Vertex start =new(GraphOrigin);
    vertices.Add(start);
    vertex_queue.Enqueue(start);

    GenerateGraphBFS();
  }

  // Called every frame. 'delta' is the elapsed time since the previous frame.
  public override void _Process(double delta)
  {
    // QueueRedraw();
  }

  private void GenerateGraphBFS()
  {
    // int i = 0;
    // while ( i < 25 && vertex_queue.Count > 0)
    while (vertex_queue.Count > 0)
    {
      // GD.Print(string.Join(", ", vertex_queue));
      GD.Print("Processing:" + vertex_queue.Peek());
      ProcessVertex(vertex_queue.Dequeue());
      // GD.Print(string.Join(", ", vertex_queue));
      GD.Print();
      // i++;
    }

  }

  private void ProcessVertex(Vertex current)
  {
    // check if vertex is visited
    if(current.Visited)
      return;
    
    // set to visited and add to graph
    current.Visit();

    // get neighbors
    Vertex[] neighbors = GetNeighboringCells(current.position, out bool[] neighbors_added);

    for(int i = 0; i < neighbors.Length; i++ )
    {
      Vertex neighbor = neighbors[i];
      
      // TODO: optimize enqueueing so it does not enter vertexes that already have been visited
      // check if the neighbor is already visited or in the queue
      if (neighbors_added[i])
        vertex_queue.Enqueue(neighbor); // register all neighbors

      // register edge to the neighbor in the hashset
      edges.Add(new(current, neighbor, neighbor.isDiagonal? 22 : 16)); // using an aproximation for the diagonal
    }
  }


  private Vertex[] GetNeighboringCells(Vector2I referenceCell, out bool[] neighbors_added)
  {
    List<Vertex> result = [];
    Vertex[] neighbors = [
        // straight paths
        new(new (referenceCell.X, referenceCell.Y - 1)), // up
        new(new (referenceCell.X, referenceCell.Y + 1)), // down
        new(new (referenceCell.X - 1, referenceCell.Y)), // left
        new(new (referenceCell.X + 1, referenceCell.Y)), // right
        // diagonals the cost should be higher
        // new(new (referenceCell.X - 1, referenceCell.Y - 1),true), // top left
        // new(new (referenceCell.X + 1, referenceCell.Y - 1),true), // top right
        // new(new (referenceCell.X - 1, referenceCell.Y + 1),true), // bottom left
        // new(new (referenceCell.X + 1, referenceCell.Y + 1) true), // bottom right
    ];
    
    neighbors_added = new bool[neighbors.Length];

    for(int i = 0; i < neighbors.Length; i++)
    {
      Vertex neighbor = neighbors[i];
      // check if valid vertex
      if (GetCellSourceId(neighbor.position) != 0) {
        continue; // not a valid neighbor
      }

      
      if (!vertices.Add(neighbor))
      {
        neighbors_added[i] = false;
        // GD.Print("Vertex could not be added: " + neighbor);
      } 
      else 
      {
        neighbors_added[i] = true;
        // GD.Print("Vertex could be added: " + neighbor);

        bool found = vertices.TryGetValue(neighbor, out Vertex actualValue);
        if (found) 
          result.Add(actualValue);
      }
    }

    return [.. result];
  }

  public override void _Draw()
  {
    base._Draw();
  }
}
