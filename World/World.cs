using Godot;
using System;
using System.Threading.Tasks;

public partial class World : Node2D
{
  [Export]
  public DebugScreen debug_ref;

  public bool visualize_debug_info = false; //debug_ref.visualize_debug_info;

  [Export]
  public MovingEntity[] movingEntity_list { get; set; }

  [Export]
  public Graph graph_ref;

  [Export]
  public bool Playing { get; set; } = false;

  [Export]
  public bool Step { get; set; } = false;

  public override void _Ready()
  {
      base._Ready();
  }
  public override void _Process(double delta)
  {
    // Alleen input "Listeners hier", game logic in het onderste deel!
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
      GD.Print("Debug screen toggle");
      debug_ref.Visible = !debug_ref.Visible;
      debug_ref.ShowDebug = true;
    }


    // stop als op pauze
    if (Engine.TimeScale == 0f)
          return;

    QueueRedraw();
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
