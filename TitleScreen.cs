using Godot;
using System;

public partial class TitleScreen : Control
{
    private Button _startButton;
    private Button _helpButton;
    private Button _backButton;
    private Control _titleScreenControl;
    private Control _helpScreenControl;

    public override void _Ready()
    {
        GetTree().Paused = false;

        _titleScreenControl = GetNode<Control>("VBoxContainer/HBoxContainer/TitleScreenControl");
        _helpScreenControl = GetNode<Control>("VBoxContainer/HBoxContainer/HelpScreenControl");

        _startButton = GetNode<Button>("VBoxContainer/HBoxContainer/TitleScreenControl/Play");
        _startButton.Pressed += () =>
        {
            GetTree().ChangeSceneToFile("res://World/world.tscn");
        };

        _helpButton = GetNode<Button>("VBoxContainer/HBoxContainer/TitleScreenControl/Help");
        _helpButton.Pressed += () =>
        {
            _titleScreenControl.Visible = false;
            _helpScreenControl.Visible = true;
        };

        _backButton = GetNode<Button>("VBoxContainer/HBoxContainer/HelpScreenControl/CloseHelp");
        _backButton.Pressed += () =>
        {
            _helpScreenControl.Visible = false;
            _titleScreenControl.Visible = true;
        };
    }
}
