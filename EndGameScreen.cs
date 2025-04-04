using Godot;
using System;

public partial class EndGameScreen : Control
{
    private Button _restartButton;
    private Label _titleLabel;
    private Globals _globals;
    public override void _Ready()
    {
        _globals = GetNode<Globals>("/root/Globals");
        _restartButton = GetNode<Button>("VBoxContainer/HBoxContainer/TitleScreenControl/Play");
        _restartButton.Pressed += () =>
        {
            GetTree().ChangeSceneToFile("res://titleScreen.tscn");
        };

        _titleLabel = GetNode<Label>("VBoxContainer/HBoxContainer/TitleScreenControl/Title");

        _titleLabel.Text =  _globals.IsGameWon ? "You Win!" : "You Lose!";
    }
}
