using Godot;
using OutCastSurvival;
using System;

public partial class DebugScreen : Control
{
  [Signal]
  public delegate void DebugOptionChangedEventHandler();
  public bool ShowDebug = false;

  private CheckButton graph_btn;
  public bool ShowGraph {get; private set;}
  private CheckButton obstacle_btn;
  public bool ShowObstacles {get; private set;}
  private CheckButton seperation_btn;
  public bool ShowSeperation {get; private set;}
  private CheckButton cohesion_btn;
  public bool ShowCohesion {get; private set;}
  private CheckButton obstacleAvoidance_btn;
  public bool ShowObstacleAvoidance {get; private set;}

  private World world_ref;

  public override void _Ready()
  {
    base._Ready();
    ShowDebug = false;
    
    world_ref = GetParent<World>();

    graph_btn = InitializeCheckButton("Graph_btn", value => {
      ShowGraph = value;
      EmitSignal(nameof(DebugOptionChanged));
      });
    obstacle_btn = InitializeCheckButton("Obstacle_btn", value => {
      ShowObstacles = value;
      EmitSignal(nameof(DebugOptionChanged));
      });
    seperation_btn = InitializeCheckButton("Seperation_btn", value => {
      ShowSeperation = value;
      EmitSignal(nameof(DebugOptionChanged));
      });
    cohesion_btn = InitializeCheckButton("Cohesion_btn", value => {
      ShowCohesion = value;
      EmitSignal(nameof(DebugOptionChanged));
      });
    obstacleAvoidance_btn = InitializeCheckButton("ObstacleAvoidance_btn", value => {
      ShowObstacleAvoidance = value;
      EmitSignal(nameof(DebugOptionChanged));
      });
  }

  private CheckButton InitializeCheckButton(string nodePath, Action<bool> onToggle)
  {
    var button = GetNode<CheckButton>(nodePath);
    // onToggle(button.ButtonPressed); // Set the initial state
    button.Pressed += () => onToggle(button.ButtonPressed); // Bind toggle logic
    return button;
  }

  public override void _Process(double delta)
  {
    base._Process(delta);
  }
}
