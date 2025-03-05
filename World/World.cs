using Godot;
using System;
using System.Threading.Tasks;

public partial class World : Node2D
{

  [Export]
  public MovingEntity[] movingEntity_list { get; set; }

  
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
    
    // stop als op pauze
    if (Engine.TimeScale == 0f)
          return;

    // Als je niet op pauze staat moet hetgene hieronder uitgevoerd worden.
          
    // foreach(Node entity in GetTree().GetNodesInGroup("Entities")) {
    //   if (entity is MovingEntity me) {
    //     me.WrapArround();
    //   }
    // }

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

  public override void _Draw() {
    base._Draw();


    // draw lines from me's to target
    for (int i = 0; i < movingEntity_list.Length; i++)
    {
      MovingEntity me = movingEntity_list[i];
      // DrawLine(me.Position, Target, Colors.Blue, 1);
    }
  }

}
