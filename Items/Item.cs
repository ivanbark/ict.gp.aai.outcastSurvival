using Godot;
using System;
using OutCastSurvival.Entities;

public partial class Item : Area2D
{
  private Node2D _infoBox;
  private bool _isInRange = false;
  private Player _player;
  public Player Player { get { return _player; } }

  public override void _Ready()
  {
    BodyEntered += OnBodyEntered;
    BodyExited += OnBodyExited;

    if (HasNode("InfoBox"))
    {
      _infoBox = GetNode<Node2D>("InfoBox");
    }
  }

  public override void _Input(InputEvent @event)
  {
    base._Input(@event);

    if (@event.IsActionPressed("collect") && _isInRange)
    {
      Collect();
      QueueFree();
    }

  }

  protected virtual void Collect()
  {
    GD.Print("Collecting item");
  }

  public void OnBodyEntered(Node2D body)
  {
    if (body is Player player)
    {
      _player = player;
      _isInRange = true;
      _infoBox.Visible = true;
    }
  }

  public void OnBodyExited(Node2D body)
  {
    if (body is Player player)
    {
      _player = null;
      _isInRange = false;
      _infoBox.Visible = false;
    }
  }
}
