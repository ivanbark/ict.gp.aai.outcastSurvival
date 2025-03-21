using System;
using System.Collections.Generic;
using System.IO;
using Godot;

public partial class Graph : TileMapLayer
{
  [Export]
  private Vector2I GraphOrigin = new(50, 50);

  // collections of vertex's
  public Dictionary<Vector2I, Vertex> graph_collection = [];

  public HashSet<Edge> edges = [];

  private readonly Queue<Vertex> vertex_queue = [];

  // Called when the node enters the scene tree for the first time.
  public override void _Ready()
  {
    if (GetCellSourceId(GraphOrigin) == -1)
      return;
    vertex_queue.Enqueue(new(GraphOrigin));
    //BFS
    GenerateGraphBFS();

    // DFS
    // GenerateGraph(GraphOrigin);
  }

  // Called every frame. 'delta' is the elapsed time since the previous frame.
  public override void _Process(double delta)
  {
    // QueueRedraw();
  }

  private void GenerateGraphBFS()
  { int i = 0;
    while ( i < 16 && vertex_queue.Count > 0)
    {
      // if(VertexesVisited >  MAX_VERTEXES)
      //   return;

      // GD.Print(string.Join(", ", vertex_queue));
      GD.Print("Processing:" + vertex_queue.Peek());
      ProcessVertex(vertex_queue.Dequeue());
      // GD.Print(string.Join(", ", vertex_queue));
      GD.Print();
      i++;
    }

  }

  private void ProcessVertex(Vertex current)
  {
    // check if vertex is visited
    if(current.Visited)
      return;
    
    // set to visited and add to graph
    current.Visit();
    graph_collection.TryAdd(current.position, current);


    // get neighbors
    Vertex[] neighbors = GetNeighboringCells(current.position);

    foreach (var neighbor in neighbors)
    {
      // check if valid vertex
      if (GetCellSourceId(neighbor.position) != 0)
        continue;

      // TODO: optimize enqueueing so it does not enter vertexes that already have been visited
      // check if the neighbor is already visited or in the queue
      // if (!neighbor.Visited || !vertex_queue.Contains(neighbor))
      if (!graph_collection.ContainsValue(neighbor) && !vertex_queue.Contains(neighbor))
        vertex_queue.Enqueue(neighbor); // register all neighbors
      
      
      // register edge to the neighbor in the hashset
      edges.Add(new(current, neighbor, neighbor.isDiagonal? 22 : 16)); // using an aproximation for the diagonal
    }
  }


  private Vertex[] GetNeighboringCells(Vector2I referenceCell)
  {
    List<Vertex> neighbors = [];

    Vector2I[] potentialNeighbors = [
        // straight paths
        new (referenceCell.X, referenceCell.Y - 1), // up
        new (referenceCell.X, referenceCell.Y + 1), // down
        new (referenceCell.X - 1, referenceCell.Y), // left
        new (referenceCell.X + 1, referenceCell.Y), // right
        // diagonals the cost should be higher
        // new (referenceCell.X - 1, referenceCell.Y - 1), // top left
        // new (referenceCell.X + 1, referenceCell.Y - 1), // top right
        // new (referenceCell.X - 1, referenceCell.Y + 1), // bottom left
        // new (referenceCell.X + 1, referenceCell.Y + 1)  // bottom right
    ];

    for (int i = 0; i < potentialNeighbors.Length; i++)
    {
      var neighbor = potentialNeighbors[i];
      if (!graph_collection.ContainsKey(neighbor))
      {
        neighbors.Add(new Vertex(neighbor, i > 3));
      }
    }

    return [.. neighbors];
  }

  public override void _Draw()
  {
    base._Draw();
  }
}
