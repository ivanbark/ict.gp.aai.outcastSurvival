using Godot;
using System;
using System.Threading.Tasks;
using System.Linq;
using OutCastSurvival.Entities;
using System.Collections.Generic;

namespace OutCastSurvival
{
  public partial class World : Node2D
  {
    [Export]
    public DebugScreen debug_ref;

    public bool visualize_debug_info = false; //debug_ref.visualize_debug_info;

    [Export]
    public MovingEntity[] movingEntity_list { get; set; }

    [Export]
    public Graph graph_ref;

    public Vertex TargetVertex = null;

    [Export]
    public bool Playing { get; set; } = true;

    [Export]
    public bool Step { get; set; } = false;

    [Export]
    public int NumberOfChests { get; set; } = 12;

    private Globals _globals;

    [Export]
    public Window fuzzyWindow_ref;

    public override void _Ready()
    {
      base._Ready();
      SpawnChests();
      _globals = GetNode<Globals>("/root/Globals");
    }

    private void SpawnChests()
    {
      var random = new Random();
      var chestScene = GD.Load<PackedScene>("res://Items/gold_chest.tscn");
      var spawnedChests = new List<Vector2>();
      const float MIN_DISTANCE = 300f; // Minimum distance between chests

      for (int i = 0; i < NumberOfChests; i++)
      {
        // Get a random walkable position from the graph
        var vertices = graph_ref.vertices.ToList();
        if (vertices.Count == 0) continue;

        Vector2 position;
        int attempts = 0;
        const int MAX_ATTEMPTS = 50;

        do
        {
          var randomVertex = vertices[random.Next(vertices.Count)];
          position = graph_ref.MapToLocal(randomVertex.position);
          attempts++;
        } while (spawnedChests.Any(p => p.DistanceTo(position) < MIN_DISTANCE) && attempts < MAX_ATTEMPTS);

        if (attempts >= MAX_ATTEMPTS) continue; // Skip if we couldn't find a valid position

        // Create and add the chest
        var chest = chestScene.Instantiate<GoldChest>();
        AddChild(chest);
        chest.Position = position;
        spawnedChests.Add(position);
      }
    }

    public override void _Process(double delta)
    {
      // Alleen input "Listeners hier", game logic in het onderste deel!
      // Engine.TimeScale = 0f;
      if (Input.IsActionJustPressed("pause_play_toggle"))
      {
        Playing = !Playing;

        UpdatePlayPauseLabel();

        if (Playing)
        {
          GD.Print("Start animation");
          Engine.TimeScale = 1f;
        }
        else
        {
          GD.Print("Stop animation");
          Engine.TimeScale = 0f;
        }
      }

      if (Input.IsActionJustPressed("step_backwards"))
      {
        if (Playing) return;
        GD.Print("Step backwards");
        Step = true;
        _ = Process_Step(-1f);
      }

      if (Input.IsActionJustPressed("step_forwards"))
      {
        if (Playing) return;
        GD.Print("Step forwards");
        Step = true;
        _ = Process_Step();
      }
      if (Input.IsActionJustPressed("visualize_debug_info"))
      {
        GD.Print("Debug screen toggle");
        debug_ref.Visible = !debug_ref.Visible;
        debug_ref.ShowDebug = !debug_ref.ShowDebug;
        debug_ref.SendGraphicsUpdate();
      }
      // listen for mouse input
      if (Input.IsActionJustPressed("mouse_click"))
      {
        GD.Print("Mouse clicked");
        Vector2 mousePosition = GetGlobalMousePosition();
        GD.Print(mousePosition);
        Vector2I vec = new((int)(mousePosition.X / graph_ref.TileSize), (int)(mousePosition.Y / graph_ref.TileSize));
        graph_ref.GetVertexForPosition(vec, out TargetVertex);

        var sheep = GetTree().GetNodesInGroup("Sheep");
        foreach (Node entity in sheep)
        {
          if (entity is Sheep sheepEntity)
          {
            sheepEntity.path = null;
            sheepEntity.pathIndex = 0;
          }
        }


        debug_ref.SendGraphicsUpdate();

      }


      if (Input.IsActionJustPressed("open_fuzzy_window"))
      {
        fuzzyWindow_ref.Visible = !fuzzyWindow_ref.Visible;
      }


      // stop als op pauze
      if (Engine.TimeScale == 0f)
        return;

      QueueRedraw();
    }

    public Player GetPlayer()
    {
      return (Player)GetNode<CharacterBody2D>("Player");
    }

    public Sheep[] GetOtherSheep(Vector2 coord, float radius)
    {
      List<Sheep> otherSheep = [];

      // get all the sheep
      var allsheep = GetTree().GetNodesInGroup("Sheep");

      // check if in the provided radius
      foreach (Node entity in allsheep)
      {
        if (entity is Sheep sheep)
        {
          if (sheep.Position.DistanceTo(coord) <= radius)
            otherSheep.Add(sheep);
        }
      }

      return [.. otherSheep];
    }

    private void UpdatePlayPauseLabel()
    {
      Label Play_Pause_label = GetNode<CanvasLayer>("UI").GetNode<Label>("PlayPause");
      if (Playing)
      {
        Play_Pause_label.Text = "";
      }
      else
      {
        Play_Pause_label.Text = "||";
      }
    }

    private async Task Process_Step(float step = 1f)
    {
      Engine.TimeScale = step;
      await ToSignal(GetTree(), "process_frame");
      Engine.TimeScale = 0f;
      Step = false;
    }

    public void GetPathTo(Vector2I start, Vector2I destination)
    {
      bool found_start = graph_ref.GetVertexForPosition(start, out Vertex startVertex);
      bool found_destination = graph_ref.GetVertexForPosition(destination, out Vertex destinationtVertex);
      if (found_start && found_destination)
      {
        GD.Print("We can start to find a path");
      }
    }

    public void EndGame(bool isWon)
    {
      _globals.EndGame(isWon);
      GetTree().Paused = true;
      GetTree().ChangeSceneToFile("res://end_game_screen.tscn");
    }

  }
}
