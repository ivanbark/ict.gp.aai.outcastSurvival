using Godot;
using System;

public partial class TitleScreen : Control
{
    private Button _startButton;

    public override void _Ready()
    {
        _startButton = GetNode<Button>("Play");
        _startButton.Pressed += () =>
        {
            GetTree().ChangeSceneToFile("res://World/world.tscn");
        };
    }
}
