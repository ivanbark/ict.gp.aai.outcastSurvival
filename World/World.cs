using Godot;
using System;
using System.Threading.Tasks;
using OutCastSurvival.Entities;
using System.Collections.Generic;

namespace OutCastSurvival 
{
public partial class World : Node2D
{
  [Export]
  public bool visualize_debug_info { get; set; } = false;

  [Export]
  public MovingEntity[] movingEntity_list { get; set; }

  [Export]
  public Graph graph_ref;

  [Export]
  public bool Playing { get; set; } = true;

  [Export]
  public bool Step { get; set; } = false;

  public override void _Ready()
  {
      base._Ready();
  }
  public override void _Process(double delta)
  {
    // Alleen input "Listeners hier", game logic in het onderste deel!
    // Engine.TimeScale = 0f;
    if (Input.IsActionJustPressed("pause_play_toggle")) {
      Playing = !Playing;

      UpdatePlayPauseLabel();

      if (Playing) {
        GD.Print("Start animation");
        Engine.TimeScale = 1f;
      } else {
        GD.Print("Stop animation");
        Engine.TimeScale = 0f;
      }
    }

    if(Input.IsActionJustPressed("step_backwards")) {
      if(Playing) return;
      GD.Print("Step backwards");
      Step = true;
      _ = Process_Step(-1f);
    }

    if(Input.IsActionJustPressed("step_forwards")) {
      if(Playing) return;
      GD.Print("Step forwards");
      Step = true;
      _ = Process_Step();
    }

    if (Input.IsActionJustPressed("visualize_debug_info")) {
      visualize_debug_info = !visualize_debug_info;
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
    var allsheep = GetTree().GetNodesInGroup("Entities");

    // check if in the provided radius
    foreach (Node entity in allsheep)
    {
      if (entity is Sheep sheep)
      {
        if ( sheep.Position.DistanceTo(coord) <= radius)
          otherSheep.Add(sheep);
      }
    }

    return [.. otherSheep];
  }

  private void UpdatePlayPauseLabel() {
    Label Play_Pause_label = GetNode<CanvasLayer>("UI").GetNode<Label>("PlayPause");
    if (Playing){
      Play_Pause_label.Text = "";
    } else {
      Play_Pause_label.Text = "||";
    }
  }

  private async Task Process_Step(float step = 1f) {
    Engine.TimeScale = step;
    await ToSignal(GetTree(), "process_frame");
    Engine.TimeScale = 0f;
    Step = false;
  }

  public override void _Draw() {
    base._Draw();


    // draw lines from me's to target
    for (int i = 0; i < movingEntity_list.Length; i++)
    {
      MovingEntity me = movingEntity_list[i];
      // DrawLine(me.Position, Target, Colors.Blue, 1);
    }
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
}
}