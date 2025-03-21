using System.Collections.Generic;
using System.IO;
using Godot;

public partial class Graph : TileMapLayer
{
	[Export]
	private Vector2I GraphOrigin  = new (50,50);


	// collections of vertex's
	// Vector2I, vertex
	public Dictionary<Vector2I,Vertex> graph_collection = new Dictionary<Vector2I, Vertex>();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GenerateGraph(GraphOrigin);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

		QueueRedraw();
	}


#nullable enable
	private Vertex? GenerateGraph(Vector2I start) {
		// get starting cell
		int id = GetCellSourceId(start);
		// if -1 now cell in that location
		if (id == -1) {
			GD.Print("No cell found at: " + start);
			return null;
		}

		Vertex start_vertex = new (start);

		if (!graph_collection.TryAdd(start, start_vertex))
			return null; // not a tile exit recursion.


		// // get the neighboring cells.
		var neighbors =  GetSurroundingCells(start);
		List<Vertex> vertex_neighbors = [];
		foreach (var neighbor in neighbors)
		{
			GD.Print(neighbor);
			if (GenerateGraph(neighbor) is Vertex neighborVertex)
			{
				vertex_neighbors.Add(neighborVertex);
			}
		}
		foreach (Vertex vertex in vertex_neighbors)
		{
			start_vertex.AddNeighbor(new(vertex));
		}

		return start_vertex;
	}

    public override void _Draw()
    {
        base._Draw();
    }
}
